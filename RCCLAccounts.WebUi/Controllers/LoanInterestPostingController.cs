using FastReport;
using Fizzler;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Core.Models.CpfDeposit;
using ProvidentFund.Core.Services;
using ProvidentFund.Data;
using ProvidentFund.Data.Entities;
using ProvidentFund.WebUi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProvidentFund.WebUi.Controllers
{
    public class LoanInterestPostingController : Controller
    {
      
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _dbContext;
        string sqlCon;
        private object Else;

        private readonly IEmployeeCpfledgerService _cpfledgerService;

        public LoanInterestPostingController(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
             AppDbContext dbContext,
             IEmployeeCpfledgerService cpfledgerRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _dbContext = dbContext;
            sqlCon = _dbContext.Database.GetDbConnection().ConnectionString;
            _cpfledgerService = cpfledgerRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                var interestPostingList = GetLoanInterestPostingData();
                return View(interestPostingList);


            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Error!!!";
                return View();
            }
        }

      

        // GET: Interest Distribution/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
    
            ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");
            var initialData = new List<ProvidentFund.Core.Models.InterestPostingModel>
    {
            new InterestPostingModel
            {
                LoanInfoId = 0,
                LoanTypeName = "",
                LoanNo = "",
                EmpolyeeName = "",
                BranchName="",
                Balance=0,
                Rate=0,
                InterestDay=0,
                MonthlyProfit=0,
                ProvisonalProfit=0,
                TotalProfit=0


            }
        };
           
            return View(initialData);
        }


        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var CPFDeposite = await _cpfledgerService.GetAllCPFDepositeByTransIdAsync(id);
            if (CPFDeposite == null)
            {
                return NotFound();
            }

            return View(CPFDeposite);
        }

        [HttpPost]
        // POST: Interest Distribution/Create
        public async Task<IActionResult> Create(List<InterestPostingModel> interestPosting)
        {

            try
            {
                          
                ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();

                var user = await _userManager.GetUserAsync(User);
                string UserName = user.FullName.ToString();

                string UserIP = GetIPV4;

                int totalRowsInserted =  await InsertInterestPosting(interestPosting, UserName, UserIP);


               return Json(totalRowsInserted);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Save!!!";
                return View(interestPosting);
            }
        }

        // POST: Loan Interest Posting/Delete/5
        //[HttpPost, ActionName("Delete")]
      // public ActionResult Delete(string id)
      public async Task<IActionResult> Delete(DefaultRequest request)

        {
            try
            {
                int intPostingId = 0;

               
                    intPostingId = DeleteLoanInterestPosting(request.Id.ToString());

                    if (intPostingId > 0)
                    {
                        //TempData["SuccessMessage"] = "Loan interest transaction deleted successfully.";
                        //TempData["title"] = "Delete!!!";
                        return Ok(new DefaultResponse("Success"));
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to delete the loan transaction.";
                        TempData["title"] = "Delete!!!";
                    }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Delete!!!";
                return View();
            }
        }

    

        #region VariousMethod
        // Validation Check

        // Insert Posting data
        public async Task<int> InsertInterestPosting(List<InterestPostingModel> interestPosting, string UserName, string UserIP)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_InsertLoanInterestPosting", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Create a TVP parameter and set its value
                var tvp = new SqlParameter("@InterestPostingTable", SqlDbType.Structured);
                tvp.Value = ConvertToDataTable(interestPosting); // Implement this method
                tvp.TypeName = "varInterestPosting"; // Specify your custom table type

                command.Parameters.Add(tvp);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@UserIP", UserIP);

                connection.Open();
                rowsAffected = await command.ExecuteNonQueryAsync();
                connection.Close();
            }
            return rowsAffected;
        }

        private DataTable ConvertToDataTable(List<InterestPostingModel> interestPosting)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("TransactionDate", typeof(DateTime));
            dataTable.Columns.Add("LoanInfoId", typeof(int));
            dataTable.Columns.Add("LoanTypeName", typeof(string));
            dataTable.Columns.Add("LoanNo", typeof(string));
            dataTable.Columns.Add("Balance", typeof(decimal));
            dataTable.Columns.Add("Rate", typeof(decimal));
            dataTable.Columns.Add("InterestDay", typeof(int));
            dataTable.Columns.Add("MonthlyProfit", typeof(decimal));
            dataTable.Columns.Add("ProvisonalProfit", typeof(decimal));
            dataTable.Columns.Add("TotalProfit", typeof(decimal));
          

            foreach (var item in interestPosting)
            {
                DataRow row = dataTable.NewRow();
                row["TransactionDate"] = item.TransactionDate;
                row["LoanInfoId"] = item.LoanInfoId;
                row["LoanTypeName"] = item.LoanTypeName;
                row["LoanNo"] = item.LoanNo;
                row["Balance"] = item.Balance;
                row["Rate"] = item.Rate;
                row["InterestDay"] = item.InterestDay;
                row["MonthlyProfit"] = item.MonthlyProfit;
                row["ProvisonalProfit"] = item.ProvisonalProfit;
                row["TotalProfit"] = item.TotalProfit;

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }


        //Get All Interest Distribution Data
        [HttpGet]
        public List<InterestPostingModel> GetLoanInterestGenerateData(string transactionDate)
        {
            List<InterestPostingModel> interestDistributionList = new List<InterestPostingModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetLoanInterestGenerateData";
                command.Parameters.AddWithValue("@TransactionDate", transactionDate);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    interestDistributionList.Add(new InterestPostingModel
                    {
                        LoanInfoId = Convert.ToInt32(dr["LoanInfoId"]),
                        LoanTypeName = dr["LoanTypeName"].ToString(),
                        LoanNo = dr["LoanNo"].ToString(),
                        EmpolyeeName = dr["EmployeeName"].ToString(),
                        BranchName = dr["BranchName"].ToString(),
                        Balance = Convert.ToDecimal(dr["Balance"]),
                        Rate = Convert.ToDecimal(dr["RateOfInterest"]),
                        InterestDay = Convert.ToInt32(dr["InterestDay"]),
                        MonthlyProfit = Convert.ToDecimal(dr["MonthlyProfit"]),
                        ProvisonalProfit = Convert.ToDecimal(dr["ProvisonalProfit"]),
                        TotalProfit = Convert.ToDecimal(dr["TotalProfit"])

                    });
                }
            }
            return interestDistributionList;

        }


        //Get All Interest Posting List  Data
      
        public List<InterestPostingModel> GetLoanInterestPostingData()
        {
            List<InterestPostingModel> interestPostingList = new List<InterestPostingModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetLoanInterestPostingList";
               
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    interestPostingList.Add(new InterestPostingModel
                    {
                        TransactionId = dr["TransactionId"].ToString(),
                        TransactionDate = (DateTime)dr["TransactionDate"],
                        LoanTypeName = dr["LoanTypeName"].ToString(),                
                        Balance = Convert.ToDecimal(dr["Balance"]),
                        InterestDay = Convert.ToInt32(dr["InterestDay"]),
                        MonthlyProfit = Convert.ToDecimal(dr["MonthlyProfit"]),
                        ProvisonalProfit = Convert.ToDecimal(dr["ProvisonalProfit"]),
                        TotalProfit = Convert.ToDecimal(dr["TotalProfit"])

                    });
                }
            }
            return interestPostingList;

        }

        //Delete Loan Inteest posting data
        public int DeleteLoanInterestPosting(string TrId)
        {
            int Id = 0;

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_DeleteLoanInterestPosting", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionId", TrId);

                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }

        #endregion



    }

}
