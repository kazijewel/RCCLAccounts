using FastReport;
using Fizzler;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Claims;

namespace RCCLAccounts.WebUi.Controllers
{
    public class CPFWithdrawController : Controller
    {
        public class EmpolyeeList
        {
            public int EmpolyeeId { get; set; }
            public string EmpolyeeName { get; set; }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _dbContext;
        string sqlCon;
        private object Else;
        private commonService commonService;
        public CPFWithdrawController(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
             AppDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _dbContext = dbContext;
            sqlCon = _dbContext.Database.GetDbConnection().ConnectionString;

            commonService = new commonService(_httpContextAccessor, _dbContext);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cpfwithdrawTransactionList = GetCPFWithdrawTransaction();

                if (cpfwithdrawTransactionList.Count == 0)
                {
                    TempData["InfoMessage"] = "Currently CPF withdraw transaction not available in the Database.";
                    TempData["title"] = "Index";
                }
                return View(cpfwithdrawTransactionList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Error!!!";
                return View();
            }
        }

        // GET: Loandisbursment/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
     
            ViewBag.Empolyee = new SelectList(GetEmpolyeeLoad(), "EmpolyeeId", "EmpolyeeName");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
             
            };
            ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");


            return View();
        }
        [HttpPost]
        // POST: CPF Withdraw/Create
        public async Task<IActionResult> Create(EmployeeCpfledgerModel EmployeeCpfledger)
        {

            try
            {
                int id = 0;

             ViewBag.Empolyee = new SelectList(GetEmpolyeeLoad(), "EmpolyeeId", "EmpolyeeName");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
        
            };
                ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();

                EmployeeCpfledger.UserIp = GetIPV4;
                EmployeeCpfledger.UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);                    
                EmployeeCpfledger.UserName = user.FullName.ToString();
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

                //  if (ModelState.IsValid)
                if (isValidModel(EmployeeCpfledger))
                {
                    id = InsertCPFWithdraw(EmployeeCpfledger);

                    if (id > 0)
                    {

                        TempData["SuccessMessage"] = "CPF withdraw transaction saved successfully.";
                        TempData["title"] = "Save!!!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to Save .";
                        TempData["title"] = "Save!!!";
                       
                    }
                }

                return View(EmployeeCpfledger);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Save!!!";
                return View(EmployeeCpfledger);
            }
        }

        //public async Task<IActionResult> Edit(int? id)
        //{


        //    ViewBag.Empolyee = new SelectList(GetEmpolyeeLoad(), "EmpolyeeId", "EmpolyeeName");

        //    ViewBag.TransactionType = new List<SelectListItem>()
        //    {
        //      new SelectListItem() { Text="Transfer",Value="Transfer"}
        //    };
        //    ViewBag.TransactionMode = new List<SelectListItem>()
        //    {
        //      new SelectListItem() { Text="Withdraw",Value="Withdraw"},
        //    };

        //    try
        //    {
        //        var loanTransactionList = GetLoanTransactionByID((int)id).FirstOrDefault();

        //        if (loanTransactionList == null)
        //        {
        //            TempData["ErrorMessage"] = "CPF withdraw  Transaction not available with the Transaction Id : " + id;
        //            TempData["title"] = "Update!!!";
        //            return RedirectToAction("Index");
        //        }
        //        else {
        //        loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
        //        ViewBag.DatumTr = loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
        //        ViewBag.DatumCheque = loanTransactionList.ChequeDate.ToString("yyyy-MM-dd");
        //        }
        //        return View(loanTransactionList);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        TempData["title"] = "Update!!!";
        //        return View();
        //    }


        //}

        //// POST: Loandisbursment/Edit/5
        //[HttpPost, ActionName("Edit")]
        //public async Task<IActionResult> Edit(EmployeeCpfledgerModel EmployeeCpfledger)
        //{
        //    try
        //    {
        //        int id = 0;

        //    ViewBag.Empolyee = new SelectList(GetEmpolyeeLoad(), "EmpolyeeId", "EmpolyeeName");

        //    ViewBag.TransactionType = new List<SelectListItem>()
        //    {
        //      new SelectListItem() { Text="Transfer",Value="Transfer"}
        //    };
        //    ViewBag.TransactionMode = new List<SelectListItem>()
        //    {
        //      new SelectListItem() { Text="Withdraw",Value="Withdraw"},

        //    };
        //        ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");

        //        var addlist = Dns.GetHostEntry(Dns.GetHostName());
        //        string GetHostName = addlist.HostName.ToString();
        //        string GetIPV6 = addlist.AddressList[0].ToString();
        //        string GetIPV4 = addlist.AddressList[1].ToString();

        //        EmployeeCpfledger.UserIp = GetIPV4;

        //        var user = await _userManager.GetUserAsync(User);
        //        EmployeeCpfledger.UserName = user.FullName.ToString();
        //        var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

        //       // if (ModelState.IsValid)
        //        if (isValidModel(EmployeeCpfledger))
        //            {
        //            id = UpdateLoanDisbursment(EmployeeCpfledger);

        //            if (id > 0)
        //            {
        //                TempData["SuccessMessage"] = "CPF withdraw  Transaction updated successfully.";
        //                TempData["title"] = "Update!!!";
        //                return RedirectToAction("Index");
        //            }
        //            else
        //            {
        //                TempData["ErrorMessage"] = "Unable to update the CPF withdraw  Transaction.";
        //                TempData["title"] = "Update!!!";
        //            }
        //        }
        //        return View(EmployeeCpfledger);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = ex.Message;
        //        TempData["title"] = "Update!!!";
        //        return View();
        //    }
        //}

        // GET: Loandisbursment/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.Empolyee = new SelectList(GetEmpolyeeLoad(), "EmpolyeeId", "EmpolyeeName");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
            };
            try
            {
                var loanTransactionList = GetCPFWithdrawTransactionByID(id).FirstOrDefault();

                if (loanTransactionList == null)
                {
                    TempData["ErrorMessage"] = "CPF withdraw  Transaction not available with the Transaction Id : " + id;
                    TempData["title"] = "Delete!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
                    ViewBag.DatumTr = loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");

                }
                return View(loanTransactionList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Delete!!!";
                return View();
            }
        }

        // POST: Loandisbursment/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                int cpfLedgerId = 0;

                if (ModelState.IsValid)
                {
                    cpfLedgerId = DeleteCPFWithdrawTransaction(id);

                    if (cpfLedgerId > 0)
                    {
                        TempData["SuccessMessage"] = "CPF withdraw transaction deleted successfully.";
                        TempData["title"] = "Delete!!!";
                        
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to delete the CPF withdraw  transaction.";
                        TempData["title"] = "Delete!!!";
                    }
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

        // GET: Loandisbursment/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                var cpfwithdrawTransactionList = GetCPFWithdrawTransactionByID(id).FirstOrDefault();

                if (cpfwithdrawTransactionList == null)
                {
                    TempData["ErrorMessage"] = "CPF withdraw  Transaction not available with the Transaction Id : " + id;
                    TempData["title"] = "Detail!!!";
                    return RedirectToAction("Index");
                }
                return View(cpfwithdrawTransactionList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Detail!!!";
                return View();
            }
        }


        #region VariousMethod
        // Validation Check

        public bool isValidModel(EmployeeCpfledgerModel EmployeeCpfledger)
        {
            
          if (EmployeeCpfledger.DrAmount == 0  )
            {
                TempData["InfoMessage"] = "Amount Should be greter then Zero !!";
                TempData["title"] = "Validation";
                return false;
            }
            return true;
        }

        

        //Delete CPF Withdraw  transaction data
        public int DeleteCPFWithdrawTransaction(string TransactionId)
        {
            int Id = 0;

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_DeleteCPFWithdrawTransaction", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionId", TransactionId);

                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }

        //Update Loan Disbursment data
        public int UpdateLoanDisbursment(EmployeeCpfledgerModel EmployeeCpfledger)
        {
            int Id = 0;
            var transactionId = Guid.NewGuid().ToString();
            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_UpdateLoanDisbursment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionId", transactionId);
                command.Parameters.AddWithValue("@TransactionDate", EmployeeCpfledger.TransactionDate);
                command.Parameters.AddWithValue("@EmpolyeeId", EmployeeCpfledger.EmpolyeeId);
                command.Parameters.AddWithValue("@TransactionType", EmployeeCpfledger.TransactionType);
                command.Parameters.AddWithValue("@TransactionMode", EmployeeCpfledger.TransactionMode);
                command.Parameters.AddWithValue("@Narration", (EmployeeCpfledger.Narration == null ? "" : EmployeeCpfledger.Narration));
                command.Parameters.AddWithValue("@DrAmount", EmployeeCpfledger.DrAmount);
                command.Parameters.AddWithValue("@UserID", EmployeeCpfledger.UserId);
                command.Parameters.AddWithValue("@UserName", EmployeeCpfledger.UserName);
                command.Parameters.AddWithValue("@UserIp", EmployeeCpfledger.UserIp);

               
                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }



        //Get Loan transaction by Id
        public List<EmployeeCpfledgerModel> GetCPFWithdrawTransactionByID(string TransactionId)
        {
            List<EmployeeCpfledgerModel> cpfwithdrawTransactionList = new List<EmployeeCpfledgerModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetCPFWithdrawTransactionByID";
                command.Parameters.AddWithValue("@TransactionId", TransactionId);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    cpfwithdrawTransactionList.Add(new EmployeeCpfledgerModel
                    {
                        TransactionId = dr["TransactionId"].ToString(),
                        EmpolyeeId = Convert.ToInt32(dr["EmpolyeeId"]),
                        TransactionDate = (DateTime)dr["TransactionDate"],
                        TransactionType = dr["TransactionType"].ToString(),
                        TransactionMode = dr["TransactionMode"].ToString(),
                        Narration = dr["Narration"].ToString(),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                        EmpolyeeName = dr["EmpolyeeName"].ToString()

                    });
                }
            }
            return cpfwithdrawTransactionList;
        }

        //Get All CPF Withdraw Transaction
        public List<EmployeeCpfledgerModel> GetCPFWithdrawTransaction()
        {
            List<EmployeeCpfledgerModel> loanTransactionList = new List<EmployeeCpfledgerModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetCPFWithdrawTransaction";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    loanTransactionList.Add(new EmployeeCpfledgerModel
                    {
                        TransactionId = dr["TransactionId"].ToString(),
                        EmpolyeeName = dr["EmployeeName"].ToString(),
                        TransactionDate = (DateTime)dr["TransactionDate"],
                        TransactionType = dr["TransactionType"].ToString(),
                        TransactionMode = dr["TransactionMode"].ToString(),
                        Narration       = dr["Narration"].ToString(),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                        CrAmount = Convert.ToDecimal(dr["CrAmount"])
                    });
                }
            }
            return loanTransactionList;

        }

        public List<EmpolyeeList> GetEmpolyeeLoad()
        {
            List<EmpolyeeList> Empolyees = new List<EmpolyeeList>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "  Select E.EmpolyeeId,(E.EmployeeNo+' - '+E.EmployeeName)EmployeeName from [dbo].[EmployeeInfos] E  order by cast(EmployeeNo as bigint)  ";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    Empolyees.Add(new EmpolyeeList
                    {
                        EmpolyeeId = Convert.ToInt32(sqlData["EmpolyeeId"]),
                        EmpolyeeName = sqlData["EmployeeName"].ToString(),
                    });
                }
            }
            finally
            {
                con.Close();
            }

            return Empolyees;

        }
        /// <summary>
        /// Employee CPF Balance
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEmployeeCPFBalance(int EmpId)
        {
            decimal Balance = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "Select  [dbo].[func_EmployeeCPFBalance](@EmpolyeeId) Balance";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EmpolyeeId", EmpId);

                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        Balance = Convert.ToDecimal(sqlData["Balance"].ToString());
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return Json(new { maxId = Balance });
        }

        //Insert CPF Withdraw Data
        public int InsertCPFWithdraw(EmployeeCpfledgerModel EmployeeCpfledger)
        {
            int Id = 0;
            var transactionId = Guid.NewGuid().ToString();
            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_InsertCPFWithdraw", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionId", transactionId);
                command.Parameters.AddWithValue("@TransactionDate", EmployeeCpfledger.TransactionDate);
                command.Parameters.AddWithValue("@EmpolyeeId", EmployeeCpfledger.EmpolyeeId);
                command.Parameters.AddWithValue("@TransactionType", EmployeeCpfledger.TransactionType);
                command.Parameters.AddWithValue("@TransactionMode", EmployeeCpfledger.TransactionMode);
                command.Parameters.AddWithValue("@Narration", (EmployeeCpfledger.Narration == null ? "" : EmployeeCpfledger.Narration));
                command.Parameters.AddWithValue("@DrAmount", EmployeeCpfledger.DrAmount);
                command.Parameters.AddWithValue("@UserID", EmployeeCpfledger.UserId);
                command.Parameters.AddWithValue("@UserName", EmployeeCpfledger.UserName);
                command.Parameters.AddWithValue("@UserIp", EmployeeCpfledger.UserIp);


                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }

        #endregion



    }

}
