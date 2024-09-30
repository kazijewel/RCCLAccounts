using Azure.Core;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models.CpfDeposit;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Models;
using System.Net;

namespace RCCLAccounts.WebUi.Controllers
{
	public class CpfDepositController : Controller
	{
		private readonly IEmpService _empService;
		private readonly IEmployeeCpfledgerService _cpfledgerService;
        private commonService commonService;
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        public CpfDepositController(IEmpService empService, IEmployeeCpfledgerService cpfledgerRepository, IHttpContextAccessor accessor,
            AppDbContext db, UserManager<ApplicationUser> userManager)
		{
			_empService = empService;
			_cpfledgerService = cpfledgerRepository;
            _accessor = accessor;
            _db = db;
            commonService = new commonService(_accessor, _db);
            _userManager = userManager;
        }
        public async Task<IActionResult> CompanyDepositIndex()
        {
			List<DepositIndexResult> companyDeposits = await _cpfledgerService.GetAllCompanyCpfledgersAsync();
            return View(companyDeposits);
        }

        public async Task<IActionResult> CompanyDeposit()
		{
			var allEmployees = await _empService.GetAllEmployeesAsync();
			return View(allEmployees);
		}

        public async Task<IActionResult> EmployeeDeposit()
        {
            var allEmployees = await _empService.GetAllEmployeesAsync();
            return View(allEmployees);
        }
        public async Task<IActionResult> EmployeeDepositIndex()
        {
            List<DepositIndexResult> companyDeposits = await _cpfledgerService.GetAllEmployeeCpfledgersAsync();
            return View(companyDeposits);
        }

        [HttpPost]
		public async Task<IActionResult> SaveEmployeeDeposit(List<DepositRequest> employeeDeposits)
		{
            decimal totalAmount = employeeDeposits.Sum(deposit => deposit.BasicSalary*(deposit.ContributionPercentage / 100));
            string fsl = commonService.getFiscalYear(new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(), "B-1");
            string VoucherNo = commonService.getVoucherNo("Journal", new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString());

            string VoucherNo2 = commonService.getVoucherNoIncreseTwo("Journal", new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString());

            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            var user = await _userManager.GetUserAsync(User);

            string Year = employeeDeposits.First().Year.ToString();
            string monthName = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).Date.ToString("MMMM");

            (var result,var transactionId) = await _cpfledgerService.SaveEmployeeCpfledgerAsync(employeeDeposits);
            if (result >= 1)
            {
                var DrVoucher = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //  ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "AL221",
                    LedgerCode = "10201003",
                    LedgerName = "Transfer Account",
                    BalanceAmount = 0,
                    DrAmount = totalAmount,
                    CrAmount = 0,
                    Narration = "Own Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "CPF Deposit",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "CPF Employee Deposit",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };

            var CrVoucher = new Voucher
            {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo,
                 // VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                   //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "LL2",
                    LedgerCode = "20101003",
                    LedgerName = "Contribution Provident Fund A/C",
                    BalanceAmount = 0,
                    DrAmount = 0,
                    CrAmount = totalAmount,
                    Narration = "Own Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "CPF Deposit",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "CPF Employee Deposit",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

            };


            var DrVoucher2 = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo2,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "AL11",
                    LedgerCode = "10201002002",
                    LedgerName = "SD-616 A/C Bankers (CPF)",
                    BalanceAmount = 0,
                    DrAmount = totalAmount,
                    CrAmount = 0,
                    Narration = "Own Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "CPF Deposit",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "CPF Employee Deposit",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };

                var CrVoucher2 = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo2,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "AL221",
                    LedgerCode = "10201003",
                    LedgerName = "Transfer Account",
                    BalanceAmount = 0,
                    DrAmount = 0,
                    CrAmount = totalAmount,
                    Narration = "Own Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "CPF Deposit",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "CPF Employee Deposit",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };

                // Add them to the DbContext
                _db.Vouchers.Add(DrVoucher);
                _db.Vouchers.Add(CrVoucher);
                _db.Vouchers.Add(DrVoucher2);
                _db.Vouchers.Add(CrVoucher2);

                // Save changes to the database
                await _db.SaveChangesAsync();


            }
            return Json(result);

        }

		[HttpPost]
		public async Task<IActionResult> SaveCompanyDeposit(List<DepositRequest> employeeDeposits)
		{
            decimal totalAmount = employeeDeposits.Sum(deposit => deposit.BasicSalary * (deposit.ContributionPercentage / 100));
            string fsl = commonService.getFiscalYear(new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(), "B-1");
            string VoucherNo = commonService.getVoucherNo("Journal", new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString());

            string VoucherNo2 = commonService.getVoucherNoIncreseTwo("Journal", new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString());

            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            var user = await _userManager.GetUserAsync(User);

            string Year = employeeDeposits.First().Year.ToString();
            string monthName = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).Date.ToString("MMMM");

            (var result, var transactionId) = await _cpfledgerService.SaveCompanyCpfledgerAsync(employeeDeposits);

            if (result >= 1)
            {
                var DrVoucher = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "AL221",
                    LedgerCode = "10201003",
                    LedgerName = "Transfer Account",
                    BalanceAmount = 0,
                    DrAmount = totalAmount,
                    CrAmount = 0,
                    Narration = "RCCL Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "RCCL Contribution",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "RCCL Contribution",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };

                var CrVoucher = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "LL2",
                    LedgerCode = "20101003",
                    LedgerName = "Contribution Provident Fund A/C",
                    BalanceAmount = 0,
                    DrAmount = 0,
                    CrAmount = totalAmount,
                    Narration = "RCCL Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "RCCL Contribution",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "RCCL Contribution",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };


                var DrVoucher2 = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo2,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "AL11",
                    LedgerCode = "10201002002",
                    LedgerName = "SD-616 A/C Bankers (CPF)",
                    BalanceAmount = 0,
                    DrAmount = totalAmount,
                    CrAmount = 0,
                    Narration = "RCCL Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "RCCL Contribution",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "RCCL Contribution",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };

                var CrVoucher2 = new Voucher
                {

                    MasterNo = "",
                    FiscalYearId = fsl,
                    VoucherNo = VoucherNo2,
                    //VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1),
                    VoucherDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)),
                    ChequeNo = "",
                    //ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, 1).ToString(),
                    ChequeDate = new DateTime(employeeDeposits.First().Year, employeeDeposits.First().Month, DateTime.DaysInMonth(employeeDeposits.First().Year, employeeDeposits.First().Month)).ToString(),
                    VoucherType = "jau",
                    LedgerId = "AL221",
                    LedgerCode = "10201003",
                    LedgerName = "Transfer Account",
                    BalanceAmount = 0,
                    DrAmount = 0,
                    CrAmount = totalAmount,
                    Narration = "RCCL Contribution for the month of " + monthName + " " + Year,
                    TransactionWith = "RCCL Contribution",
                    CostCenterId = "U-1",
                    CostCenterName = "RCCL",
                    ProductId = "",
                    ProductName = "",
                    ChequeClear = 1,
                    AuditApprove = 1,
                    AuditBy = user.FullName,
                    AuditTime = DateTime.Now,
                    AuditIp = GetIPV4,
                    ApproveBy = user.FullName,
                    ApproveTime = DateTime.Now,
                    ApproveIp = GetIPV4,
                    AttachBill = "",
                    AttachCheque = "",
                    AttachReference = "",
                    ReferenceNo = "",
                    ReferenceDetails = "",
                    TransactionType = "Transfer",
                    BankName = "",
                    BranchName = "",
                    CompanyId = "B-1",
                    EntryFrom = "RCCL Contribution",
                    UserName = user.FullName,
                    UserIp = GetIPV4,
                    EntryTime = DateTime.Now,
                    TransactionId = transactionId.ToString()

                };

                // Add them to the DbContext
                _db.Vouchers.Add(DrVoucher);
                _db.Vouchers.Add(CrVoucher);
                _db.Vouchers.Add(DrVoucher2);
                _db.Vouchers.Add(CrVoucher2);

                // Save changes to the database
                await _db.SaveChangesAsync();


            }

            return Json(result);
		}
        public async Task<IActionResult> CompanyDetails(string? id)
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
        public async Task<IActionResult> EmployeeDetails(string? id)
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
        public async Task<IActionResult> Delete(string Id)
        {
            var cpfledger = await _cpfledgerService.GetByIdAsync(Id.ToString());
            var voucherDelete = _db.Vouchers.Where(item => item.TransactionId == Id.ToString() ).ToList();
            
            if (cpfledger != null)
            {
                await _cpfledgerService.DeleteAsync(Id.ToString());
                _db.Vouchers.RemoveRange(voucherDelete);
                _db.SaveChanges();

            }
            return Ok(new DefaultResponse("Success"));
        }
    }
}
