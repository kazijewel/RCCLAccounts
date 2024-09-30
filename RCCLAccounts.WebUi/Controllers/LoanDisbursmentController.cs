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
    public class LoanDisbursmentController : Controller
    {
        public class LoanList
        {
            public int LoanId { get; set; }
            public string LoanNo { get; set; }
        }

      
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;

        private readonly AppDbContext _dbContext;
        string sqlCon;
        private object Else;
        private commonService commonService;
        public LoanDisbursmentController(
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
                var loanTransactionList = GetLoanDisbursmentTransaction();

                if (loanTransactionList.Count == 0)
                {
                    TempData["InfoMessage"] = "Currently loan transaction not available in the Database.";
                    TempData["title"] = "Index";
                }
                return View(loanTransactionList);
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
     
            ViewBag.LoanAccount = new SelectList(GetLoanLoad(), "LoanId", "LoanNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Cash",Value="Cash"},
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Receipt",Value="Receipt"},
              new SelectListItem() { Text="Payment",Value="Payment"},
              new SelectListItem() { Text="Adjustment",Value="Adjustment"}
            };
            ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");


            return View();
        }
        [HttpPost]
        // POST: Loandisbursment/Create
        public async Task<IActionResult> Create(CPFLoanLedgerModel CPFLoanLedger)
        {

            try
            {
                int id = 0;

            ViewBag.LoanAccount = new SelectList(GetLoanLoad(), "LoanId", "LoanNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Cash",Value="Cash"},
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Receipt",Value="Receipt"},
              new SelectListItem() { Text="Payment",Value="Payment"},
              new SelectListItem() { Text="Adjustment",Value="Adjustment"}
            };
                ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();

                CPFLoanLedger.UserIp = GetIPV4;

                var user = await _userManager.GetUserAsync(User);
                CPFLoanLedger.UserName = user.FullName.ToString();
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

                //  if (ModelState.IsValid)
                if (isValidModel(CPFLoanLedger))
                {
                    id = InsertLoanDisbursment(CPFLoanLedger);

                    if (id > 0)
                    {

                        TempData["SuccessMessage"] = "Loan transaction saved successfully.";
                        TempData["title"] = "Save!!!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to Save .";
                        TempData["title"] = "Save!!!";
                       
                    }
                }

                return View(CPFLoanLedger);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Save!!!";
                return View(CPFLoanLedger);
            }
        }

        public async Task<IActionResult> Edit(string? id)
        {

            ViewBag.LoanAccount = new SelectList(GetLoanLoad(), "LoanId", "LoanNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Cash",Value="Cash"},
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Receipt",Value="Receipt"},
              new SelectListItem() { Text="Payment",Value="Payment"},
              new SelectListItem() { Text="Adjustment",Value="Adjustment"}
            };

            try
            {
                var loanTransactionList = GetLoanTransactionByID(id).FirstOrDefault();

                if (loanTransactionList == null)
                {
                    TempData["ErrorMessage"] = "Loan Transaction not available with the Transaction Id : " + id;
                    TempData["title"] = "Update!!!";
                    return RedirectToAction("Index");
                }
                else {
                loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
                ViewBag.DatumTr = loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
                ViewBag.DatumCheque = loanTransactionList.ChequeDate.ToString("yyyy-MM-dd");
                }
                return View(loanTransactionList);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Update!!!";
                return View();
            }


        }

        // POST: Loandisbursment/Edit/5
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(CPFLoanLedgerModel CPFLoanLedger)
        {
            try
            {
                int id = 0;
            
            ViewBag.LoanAccount = new SelectList(GetLoanLoad(), "LoanId", "LoanNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Cash",Value="Cash"},
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Receipt",Value="Receipt"},
              new SelectListItem() { Text="Payment",Value="Payment"},
              new SelectListItem() { Text="Adjustment",Value="Adjustment"}
            };
                ViewBag.Datum = DateTime.Now.ToString("yyyy-MM-dd");

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();

                CPFLoanLedger.UserIp = GetIPV4;

                var user = await _userManager.GetUserAsync(User);
                CPFLoanLedger.UserName = user.FullName.ToString();
                var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

               // if (ModelState.IsValid)
                if (isValidModel(CPFLoanLedger))
                    {
                    id = UpdateLoanDisbursment(CPFLoanLedger);

                    if (id > 0)
                    {
                        TempData["SuccessMessage"] = "Loan Transaction updated successfully.";
                        TempData["title"] = "Update!!!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to update the Loan Transaction.";
                        TempData["title"] = "Update!!!";
                    }
                }
                return View(CPFLoanLedger);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                TempData["title"] = "Update!!!";
                return View();
            }
        }

        // GET: Loandisbursment/Delete/5
        public ActionResult Delete(string id)
        {
            ViewBag.LoanAccount = new SelectList(GetLoanLoad(), "LoanId", "LoanNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Cash",Value="Cash"},
              new SelectListItem() { Text="Transfer",Value="Transfer"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Receipt",Value="Receipt"},
              new SelectListItem() { Text="Payment",Value="Payment"},
              new SelectListItem() { Text="Adjustment",Value="Adjustment"}
            };
            try
            {
                var loanTransactionList = GetLoanTransactionByID(id).FirstOrDefault();

                if (loanTransactionList == null)
                {
                    TempData["ErrorMessage"] = "Loan Transaction not available with the Transaction Id : " + id;
                    TempData["title"] = "Delete!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
                    ViewBag.DatumTr = loanTransactionList.TransactionDate.ToString("yyyy-MM-dd");
                    ViewBag.DatumCheque = loanTransactionList.ChequeDate.ToString("yyyy-MM-dd");
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
                    cpfLedgerId = DeleteLoanDisbursment(id);

                    if (cpfLedgerId > 0)
                    {
                        TempData["SuccessMessage"] = "Loan transaction deleted successfully.";
                        TempData["title"] = "Delete!!!";
                        
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to delete the loan transaction.";
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
        public ActionResult Details(String id)
        {
            try
            {
                var loanTransactionList = GetLoanTransactionByID(id).FirstOrDefault();

                if (loanTransactionList == null)
                {
                    TempData["ErrorMessage"] = "Loan Transaction not available with the Transaction Id : " + id;
                    TempData["title"] = "Detail!!!";
                    return RedirectToAction("Index");
                }
                return View(loanTransactionList);
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

        public bool isValidModel(CPFLoanLedgerModel CPFLoanLedge)
        {
            
          if (CPFLoanLedge.Amount == 0  )
            {
                TempData["InfoMessage"] = "Amount Should be greter then Zero !!";
                TempData["title"] = "Validation";
                return false;
            }
            return true;
        }

        

        //Delete Loan transaction data
        public int DeleteLoanDisbursment(string transactionId)
        {
            int Id = 0;

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_DeleteLoanDisbursment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionId", transactionId);

                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }

        //Update Loan Disbursment data
        public int UpdateLoanDisbursment(CPFLoanLedgerModel CPFLoanLedger)
        {
            int Id = 0;

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_UpdateLoanDisbursment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CPFLedgerId", CPFLoanLedger.CPFLedgerId);
                command.Parameters.AddWithValue("@TransactionDate", CPFLoanLedger.TransactionDate);
                command.Parameters.AddWithValue("@LoanInfoId", CPFLoanLedger.LoanInfoId);
                command.Parameters.AddWithValue("@TransactionType", CPFLoanLedger.TransactionType);
                command.Parameters.AddWithValue("@TransactionMode", CPFLoanLedger.TransactionMode);

                command.Parameters.AddWithValue("@ChequeNo", (CPFLoanLedger.ChequeNo == null ? "" : CPFLoanLedger.ChequeNo));
                command.Parameters.AddWithValue("@ChequeDate", CPFLoanLedger.ChequeDate);
                command.Parameters.AddWithValue("@Narration", (CPFLoanLedger.Narration == null ? "" : CPFLoanLedger.Narration));
                command.Parameters.AddWithValue("@Amount", CPFLoanLedger.Amount);
                command.Parameters.AddWithValue("@UserName", CPFLoanLedger.UserName);
                command.Parameters.AddWithValue("@UserIp", CPFLoanLedger.UserIp);

                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }



        //Get Loan transaction by Id
        public List<CPFLoanLedgerModel> GetLoanTransactionByID(string transactinId)
        {
            List<CPFLoanLedgerModel> loanTransactionList = new List<CPFLoanLedgerModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetLoanDisbursmentByID";
                command.Parameters.AddWithValue("@TransactionId", transactinId);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    loanTransactionList.Add(new CPFLoanLedgerModel
                    {
                        CPFLedgerId = Convert.ToInt32(dr["CPFLedgerId"]),
                        LoanInfoId = Convert.ToInt32(dr["LoanInfoId"]),
                        TransactionDate = (DateTime)dr["TransactionDate"],
                        TransactionType = dr["TransactionType"].ToString(),
                        TransactionMode = dr["TransactionMode"].ToString(),
                        ChequeNo = dr["ChequeNo"].ToString(),
                        ChequeDate = (DateTime)dr["ChequeDate"],
                        Narration = dr["Narration"].ToString(),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                        CrAmount = Convert.ToDecimal(dr["CrAmount"]),
                        Amount = Convert.ToDecimal(dr["Amount"]),
                        LoanDetails = dr["LoanNo"].ToString(),
                        TransactionId= dr["TransactionId"].ToString(),
                        InterestAmount = Convert.ToDecimal(dr["InterestAmount"]),
                        Suspense = Convert.ToDecimal(dr["SusInterestAmount"]),

                    });
                }
            }
            return loanTransactionList;
        }

        //Get All Loan Transaction
        public List<CPFLoanLedgerModel> GetLoanDisbursmentTransaction()
        {
            List<CPFLoanLedgerModel> loanTransactionList = new List<CPFLoanLedgerModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetLoanDisbursmentTransaction";
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    loanTransactionList.Add(new CPFLoanLedgerModel
                    {
                        CPFLedgerId = Convert.ToInt32(dr["CPFLedgerId"]),
                        LoanDetails = dr["LoanNo"].ToString(),
                        TransactionDate = (DateTime)dr["TransactionDate"],
                        TransactionType = dr["TransactionType"].ToString(),
                        TransactionMode = dr["TransactionMode"].ToString(),
                        Narration       = dr["Narration"].ToString(),
                        DrAmount = Convert.ToDecimal(dr["DrAmount"]),
                        CrAmount = Convert.ToDecimal(dr["CrAmount"]),
                        TransactionId = dr["TransactionId"].ToString(),
                    });
                }
            }
            return loanTransactionList;

        }

        public List<LoanList> GetLoanLoad()
        {
            List<LoanList> loans = new List<LoanList>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = " Select L.LoanInfoId,(L.LoanNo+' - '+E.EmployeeName)LoanNo from [dbo].[LoanInformation] L inner join[dbo].[EmployeeInfos] E on L.EmpolyeeId = E.EmpolyeeId order by LoanInfoId ";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    loans.Add(new LoanList
                    {
                        LoanId = Convert.ToInt32(sqlData["LoanInfoId"]),
                        LoanNo = sqlData["LoanNo"].ToString(),
                    });
                }
            }
            finally
            {
                con.Close();
            }

            return loans;

        }

        //Insert product data
        public int InsertLoanDisbursment(CPFLoanLedgerModel CPFLoanLedger)
        {
            int Id = 0;

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = new SqlCommand("sp_InsertLoanDisbursment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionDate", CPFLoanLedger.TransactionDate);
                command.Parameters.AddWithValue("@LoanInfoId", CPFLoanLedger.LoanInfoId);
                command.Parameters.AddWithValue("@TransactionType", CPFLoanLedger.TransactionType);
                command.Parameters.AddWithValue("@TransactionMode", CPFLoanLedger.TransactionMode);

                command.Parameters.AddWithValue("@ChequeNo", (CPFLoanLedger.ChequeNo == null ? "" : CPFLoanLedger.ChequeNo));
                command.Parameters.AddWithValue("@ChequeDate", CPFLoanLedger.ChequeDate);
                command.Parameters.AddWithValue("@Narration", (CPFLoanLedger.Narration == null ? "" : CPFLoanLedger.Narration));
                command.Parameters.AddWithValue("@Amount", CPFLoanLedger.Amount);
                command.Parameters.AddWithValue("@UserName", CPFLoanLedger.UserName);
                command.Parameters.AddWithValue("@UserIp", CPFLoanLedger.UserIp);
                command.Parameters.AddWithValue("@SusInterestAmount", CPFLoanLedger.Suspense);
                command.Parameters.AddWithValue("@InterestAmount", CPFLoanLedger.InterestAmount);

                connection.Open();
                Id = command.ExecuteNonQuery();
                connection.Close();
            }
            return Id;
        }

        //Get Loan Fincial Data
        public List<CPFLoanLedgerModel> GetLoanFinancialDetails(int loanInfoId,string transactionDate)
        {
            List<CPFLoanLedgerModel> loanTransactionList = new List<CPFLoanLedgerModel>();

            using (SqlConnection connection = new SqlConnection(sqlCon))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_GetLoanFinancialDetails";
                command.Parameters.AddWithValue("@LoanInfoId", loanInfoId);
                command.Parameters.AddWithValue("@TransactionDate", transactionDate);
                SqlDataAdapter sqlDA = new SqlDataAdapter(command);
                DataTable dtProducts = new DataTable();

                connection.Open();
                sqlDA.Fill(dtProducts);
                connection.Close();

                foreach (DataRow dr in dtProducts.Rows)
                {
                    loanTransactionList.Add(new CPFLoanLedgerModel
                    {                       
                       
                        Balance = Convert.ToDecimal(dr["Balance"]),
                        Suspense = Convert.ToDecimal(dr["SusInterestAmount"]),
                        InterestAmount = Convert.ToDecimal(dr["InterestAmount"])
                    });
                }
            }
            return loanTransactionList;

        }










        #endregion



    }

}
