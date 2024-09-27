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
    public class RCCLInterestDistributionController : Controller
    {
      

        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _dbContext;
        string sqlCon;
        private object Else;

        private readonly IEmployeeCpfledgerService _cpfledgerService;

        public RCCLInterestDistributionController(
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
                List<InterestDistributionIndexResult> interestDistribution = await _cpfledgerService.GetAllRCCLInterestDistributionCpfledgersAsync();
                return View(interestDistribution);


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

            var interestDistributionList = GetRCCLInterestDistributionData();

           
            return View(interestDistributionList);
        }


        [HttpPost]
        public async Task<IActionResult> SaveRCCLInterestDistribution(List<InterestDistributionRequest> interestDeposits)
        {

            var result = await _cpfledgerService.SaveRCCLInterestDistributionAsync(interestDeposits);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var cpfledger = await _cpfledgerService.GetByIdAsync(Id.ToString());
            if (cpfledger != null)
            {
                await _cpfledgerService.DeleteAsync(Id.ToString());
            }
            return Ok(new DefaultResponse("Success"));
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
        public async Task<IActionResult> Create(List<InterestDistributionRequest> interestDeposits)
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

                int totalRowsInserted =  await RCCLInsertInterestDistribution(interestDeposits, UserName, UserIP);


                //var user = await _userManager.GetUserAsync(User);
                //CPFLoanLedger.UserName = user.FullName.ToString();
                //var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                //  if (ModelState.IsValid)
                //if (isValidModel(CPFLoanLedger))
                //{
                //    id = InsertLoanDisbursment(CPFLoanLedger);

                //    if (id > 0)
                //    {
                //        TempData["SuccessMessage"] = "Loan transaction saved successfully.";
                //        TempData["title"] = "Save!!!";
                //        return RedirectToAction("Index");
                //    }
                //    else
                //    {
                //        TempData["ErrorMessage"] = "Unable to Save .";
                //        TempData["title"] = "Save!!!";

                //    }
                //}

                //  return View(interestDeposits);

               return Json(totalRowsInserted);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Save!!!";
                return View(interestDeposits);
            }
        }
        // Insert Interest data
        public async Task<int> RCCLInsertInterestDistribution(List<InterestDistributionRequest> interestDeposits,string UserName, string UserIP)
        {
            int totalRowsInserted = 0;
            var transactionId = Guid.NewGuid().ToString();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
               
                connection.Open();
               

                    foreach (InterestDistributionRequest interest in interestDeposits)
                {
                    using (SqlCommand command = new SqlCommand("sp_RCCLInsertInterestDistribution", connection))

                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Year", interest.Year);
                        command.Parameters.AddWithValue("@EmployeeId", interest.EmployeeId);
                        command.Parameters.AddWithValue("@BasicSalary", interest.BasicSalary);
                        command.Parameters.AddWithValue("@InterestPercentage", interest.InterestPercentage);
                        command.Parameters.AddWithValue("@InterestAmount", interest.InterestAmount);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@UserIP", UserIP);
                        command.Parameters.AddWithValue("@TransactionDate", interest.TransactionDate);
                        command.Parameters.AddWithValue("@TransactionId", transactionId);

                        totalRowsInserted += command.ExecuteNonQuery();

                    }
                               
                }
            
                connection.Close();
            }

            return totalRowsInserted;
        }



        #region VariousMethod
        // Validation Check




        //Get All Interest Distribution Data
        public List<EmployeeCpfledgerModel> GetRCCLInterestDistributionData()
        {
            List<EmployeeCpfledgerModel> interestDistributionList = new List<EmployeeCpfledgerModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetRCCLInterestDistributionInfo";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    interestDistributionList.Add(new EmployeeCpfledgerModel
                    {
                        EmpolyeeId = Convert.ToInt32(dr["EmpolyeeId"]),
                        EmpolyeeNo = dr["EmployeeNo"].ToString(),
                        EmpolyeeName = dr["EmployeeName"].ToString(),
                        BranchName = dr["BranchName"].ToString(),
                        BasicSalary = Convert.ToDecimal(dr["Balance"]),
                        ContributionPercentage = Convert.ToDecimal(dr["PerInterest"]),
                        DrAmount = Convert.ToDecimal(dr["InterestAmount"])

                    });
                }
            }
            return interestDistributionList;

        }

   

     
        #endregion



    }

}
