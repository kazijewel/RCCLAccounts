using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Models;

namespace RCCLAccounts.WebUi.Controllers
{
    public class LoanInformationController : Controller
    {
        private readonly ILoanInformationService _loanInformationService;
        private readonly IEmployeeCPFOpeningService _employeeCPFOpeningService;
        private string sqlCon;
        private AppDbContext _db;
        public LoanInformationController(ILoanInformationService loanInformationService,
              IEmployeeCPFOpeningService employeeCPFOpeningService, AppDbContext db)
        {
            _loanInformationService = loanInformationService;
            _employeeCPFOpeningService = employeeCPFOpeningService;
            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<IActionResult> Index()
        {
            var loanInformation = await _loanInformationService.GetAllLoanInfoAsync();
            return View(loanInformation);
        }
        public async Task<IActionResult> CreateAsync()
        {
            List<SelectListItem> LoanTypeName = new()
            {
                new SelectListItem { Value = "SOD", Text = "SOD" },
              
            };

            //assigning SelectListItem to view Bag
            ViewBag.LoanTypeNames = LoanTypeName;
            //ViewBag.Employee = new SelectList(await _loanInformationService.GetAllEmployeeAsync(), "EmpolyeeId", "EmployeeName");
            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            ViewBag.Bank = new SelectList(await _loanInformationService.GetAllBankAsync(), "BankId", "BankNames");
            ViewBag.BankBranch = new SelectList(await _loanInformationService.GetAllBankBranchAsync(), "BankBranchId", "BankBranchName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("LoanInfoId,LoanTypeName,LoanNo,EmpolyeeId,BankId,BankBranchId,SenctionDate,SenctionAmount,ParticularsOfSecurity,mSecurityValue,LoanPurpose,ExpiryDate,RateOfInterest,NoOfInstallment,AmountPerInstallment,DurationMonth,RecommendingOfficerName,FieldOfficerName,CalculationMethod,AccountStatus,TransactionDate,NewOld,mOpAmount,LastTransDate,SusInterestAmount")] LoanInformationModel loanInformation)
        {
           
            DateTime ct = loanInformation.SenctionDate;
            DateTime future = ct.AddMonths(loanInformation.DurationMonth);
            loanInformation.ExpiryDate = future;
            loanInformation.TransactionDate = DateTime.Now;


            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {
                await _loanInformationService.CreateAsync(loanInformation);
                TempData["AlertMessage"] = "Loan Information Create Successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(loanInformation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanInformation = await _loanInformationService.GetByLoanIdAsync(id);
            List<SelectListItem> LoanTypeName = new()
            {
                new SelectListItem { Value = "SOD", Text = "SOD" },

            };

            //assigning SelectListItem to view Bag
            ViewBag.LoanTypeNames = LoanTypeName;
            //  ViewBag.Employee = new SelectList(await _loanInformationService.GetAllEmployeeAsync(), "EmpolyeeId", "EmployeeName");

            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            ViewBag.Bank = new SelectList(await _loanInformationService.GetAllBankAsync(), "BankId", "BankNames");
            ViewBag.BankBranch = new SelectList(await _loanInformationService.GetAllBankBranchAsync(), "BankBranchId", "BankBranchName");

            if (loanInformation == null)
            {
                return NotFound();
            }
            return View(loanInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("LoanInfoId,LoanTypeName,LoanNo,EmpolyeeId,BankId,BankBranchId,SenctionDate,SenctionAmount,ParticularsOfSecurity,mSecurityValue,LoanPurpose,ExpiryDate,RateOfInterest,NoOfInstallment,AmountPerInstallment,DurationMonth,RecommendingOfficerName,FieldOfficerName,CalculationMethod,AccountStatus,TransactionDate,NewOld,mOpAmount,LastTransDate,SusInterestAmount")] LoanInformationModel loanInformation)
        {
            DateTime ct = loanInformation.SenctionDate;
            DateTime future = ct.AddMonths(loanInformation.DurationMonth);
            loanInformation.ExpiryDate = future;
            loanInformation.TransactionDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                
                try
                {
                    await _loanInformationService.UpdateAsync(loanInformation);
                    TempData["AlertMessage"] = "Loan Information Update Successfully!!!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await LoanInfoExistsAsync(loanInformation.LoanInfoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(loanInformation);
        }

        private async Task<bool> LoanInfoExistsAsync(int id)
        {
            return await _loanInformationService.LoanInfoExistsAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DefaultRequest request)
        {
            var loanInformation = await _loanInformationService.GetByLoanIdAsync(request.Id);
            if (loanInformation != null)
            {
                await _loanInformationService.DeleteAsync(request.Id);
            }
            return Ok(new DefaultResponse("Success"));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loanInformation = await _loanInformationService.GetByLoanIdAsync(id);
            if (loanInformation == null)
            {
                return NotFound();
            }

            return View(loanInformation);
        }
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

    }

}
