using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Models;
using System.Linq.Expressions;
using System.Net;
using System.Xml;

namespace RCCLAccounts.WebUi.Controllers
{
    public class BankDepositController : Controller
    {
        public IBankDepositService bankservice { get; set; }
        private commonService commonService;
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        public BankDepositController(IBankDepositService _bankservice,
            IHttpContextAccessor accessor,
            AppDbContext db,
            UserManager<ApplicationUser> userManager
            )
        {
            this.bankservice = _bankservice;
            _accessor = accessor;
            _db = db;
            commonService = new commonService( _accessor, _db);
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var allTransaction = await bankservice.GetAllBankDeposit();
            return View(allTransaction);
        }

        [HttpGet]
        public async Task<IActionResult> Create()

        {
            ViewBag.AccountInfo = new SelectList(await bankservice.GetBankAccount(), "BankAcInfoId", "AccountNo");
            
            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"}
             // new SelectListItem() { Text="Receive",Value="Receive"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Deposit",Value="Deposit"},
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
              new SelectListItem() { Text="Received",Value="Received"},
              new SelectListItem() { Text="Payment",Value="Payment"}
            };
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BankDepositVM model)
        {
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            var user = await _userManager.GetUserAsync(User);

            string fsl = commonService.getFiscalYear(model.TransactionDate.ToString(),"B-1");
            string VoucherNo = commonService.getVoucherNo("Journal", model.TransactionDate.ToString());

            model.DrAmount = 0;
            model.CrAmount = 0;

            model.VoucherNo = VoucherNo;

            string DrLedgerId = "";
            string DrLedgerCode = "";
            string DrLedgerName = "";

            string CrLedgerId = "";
            string CrLedgerCode = "";
            string CrLedgerName = "";

            if (model.TransactionMode == "Deposit")
            {
                model.DrAmount = model.Amount;
                DrLedgerId = "AL12";
                DrLedgerCode = "10201002003";
                DrLedgerName = "FDR Account- Bankers A/C (CPF)";

                CrLedgerId = "AL221";
                CrLedgerCode = "10201003";
                CrLedgerName = "Transfer Account";

            }
            else if (model.TransactionMode == "Withdraw")
            {
                model.CrAmount = model.Amount;
                DrLedgerId = "AL221";
                DrLedgerCode = "10201003";
                DrLedgerName = "Transfer Account";

                CrLedgerId = "AL11";
                CrLedgerCode = "10201002002";
                CrLedgerName = "SD-616 A/C Bankers (CPF)";
            }
            else if (model.TransactionMode == "Received")
            {
                model.CrAmount = model.Amount;
                DrLedgerId = "AL221";
                DrLedgerCode = "10201003";
                DrLedgerName = "Transfer Account";

                CrLedgerId = "LL1";
                CrLedgerCode = "20101002";
                CrLedgerName = "SOD A/C -308";
            }
            else if (model.TransactionMode == "Payment")
            {
                model.DrAmount = model.Amount;
                DrLedgerId = "LL1";
                DrLedgerCode = "20101002";
                DrLedgerName = "SOD A/C -308";

                CrLedgerId = "AL221";
                CrLedgerCode = "10201003";
                CrLedgerName = "Transfer Account";
            }

            if (ModelState.IsValid)
            {
                int ResultID = 0;

                ResultID =  await bankservice.CreateAsync(model);

                if (ResultID > 0) {

                    // Create two entities
                    var DrVoucher = new Voucher {

                        MasterNo = model.AccountId.ToString(),
                        FiscalYearId = fsl,
                        VoucherNo = VoucherNo,
                        VoucherDate = model.TransactionDate,
                        ChequeNo = "",
                        ChequeDate = model.TransactionDate.ToString(),
                        VoucherType = "jau",
                        LedgerId = DrLedgerId,
                        LedgerCode = DrLedgerCode,
                        LedgerName = DrLedgerName,
                        BalanceAmount = 0,
                        DrAmount = model.Amount,
                        CrAmount = 0,
                        Narration = model.Particulars,
                        TransactionWith = model.TransactionMode,
                        CostCenterId ="U-1",
                        CostCenterName="RCCL",
                        ProductId="",
                        ProductName="",
                        ChequeClear=1,
                        AuditApprove=1,
                        AuditBy = user.FullName,
                        AuditTime = DateTime.Now,
                        AuditIp= GetIPV4,
                        ApproveBy = user.FullName,
                        ApproveTime = DateTime.Now,
                        ApproveIp =GetIPV4,
                        AttachBill="",
                        AttachCheque="",
                        AttachReference="",
                        ReferenceNo="",
                        ReferenceDetails="",
                        TransactionType= "Transfer",
                        BankName="",
                        BranchName="",
                        CompanyId="B-1",
                        EntryFrom="Bank Deposite & Withdraw",
                        UserName= user.FullName,
                        UserIp= GetIPV4,
                        EntryTime=DateTime.Now,
                        TransactionId= ResultID.ToString()

                    };

                    var CrVoucher = new Voucher
                    {

                        MasterNo = model.AccountId.ToString(),
                        FiscalYearId = fsl,
                        VoucherNo = VoucherNo,
                        VoucherDate = model.TransactionDate,
                        ChequeNo = "",
                        ChequeDate = model.TransactionDate.ToString(),
                        VoucherType = "jau",
                        LedgerId = CrLedgerId,
                        LedgerCode = CrLedgerCode,
                        LedgerName = CrLedgerName,
                        BalanceAmount = 0,
                        DrAmount = 0,
                        CrAmount = model.Amount,
                        Narration = model.Particulars,
                        TransactionWith = model.TransactionMode,
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
                        EntryFrom = "Bank Deposite & Withdraw",
                        UserName = user.FullName,
                        UserIp = GetIPV4,
                        EntryTime = DateTime.Now,
                        TransactionId = ResultID.ToString()

                    };

                    // Add them to the DbContext
                    _db.Vouchers.Add(DrVoucher);
                    _db.Vouchers.Add(CrVoucher);
                
                    // Save changes to the database
                    await _db.SaveChangesAsync();
                }

                return RedirectToAction("Index");

            }

            ViewBag.AccountInfo = new SelectList(await bankservice.GetBankAccount(), "BankAcInfoId", "AccountNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"},
              //new SelectListItem() { Text="Receive",Value="Receive"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Deposit",Value="Deposit"},
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
              new SelectListItem() { Text="Received",Value="Received"},
              new SelectListItem() { Text="Payment",Value="Payment"}
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(DefaultRequest request)
        {
            var employee = await bankservice.GetByIdAsync(request.Id);
            var voucherDelete = _db.Vouchers.Where(item => item.TransactionId == request.Id.ToString() && item.EntryFrom== "Bank Deposite & Withdraw").ToList();

            if (employee != null && voucherDelete != null)
            {
                await bankservice.DeleteAsync(request.Id);
                _db.Vouchers.RemoveRange(voucherDelete);
                _db.SaveChanges();
            }
            return Ok(new DefaultResponse("Success"));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankdeposit = await bankservice.GetByIdAsync(id);
            if (bankdeposit == null)
            {
                return NotFound();
            }

            ViewBag.AccountInfo = new SelectList(await bankservice.GetBankAccount(), "BankAcInfoId", "AccountNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"},
              //new SelectListItem() { Text="Receive",Value="Receive"}
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Deposit",Value="Deposit"},
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
              new SelectListItem() { Text="Received",Value="Received"},
              new SelectListItem() { Text="Payment",Value="Payment"}
            };

            return View(bankdeposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BankDepositVM model)
        {
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            var user = await _userManager.GetUserAsync(User);

            string fsl = commonService.getFiscalYear(model.TransactionDate.ToString(), "B-1");
            string VoucherNo = commonService.getVoucherNo("Journal", model.TransactionDate.ToString());

            model.DrAmount = 0;
            model.CrAmount = 0;

            model.VoucherNo = VoucherNo;
            string DrLedgerId = "";
            string DrLedgerCode = "";
            string DrLedgerName = "";

            string CrLedgerId = "";
            string CrLedgerCode = "";
            string CrLedgerName = "";

            if (model.TransactionMode == "Deposit")
            {
                model.DrAmount = model.Amount;
                DrLedgerId = "AL12";
                DrLedgerCode = "10201002003";
                DrLedgerName = "FDR Account- Bankers A/C (CPF)";

                CrLedgerId = "AL221";
                CrLedgerCode = "10201003";
                CrLedgerName = "Transfer Account";
            }
            else if (model.TransactionMode == "Withdraw")
            {
                model.CrAmount = model.Amount;
                DrLedgerId = "AL221";
                DrLedgerCode = "10201003";
                DrLedgerName = "Transfer Account";

                CrLedgerId = "AL11";
                CrLedgerCode = "10201002002";
                CrLedgerName = "SD-616 A/C Bankers (CPF)";
            }
             else if (model.TransactionMode == "Received")
            {
                model.CrAmount = model.Amount;
                DrLedgerId = "AL221";
                DrLedgerCode = "10201003";
                DrLedgerName = "Transfer Account";

                CrLedgerId = "LL1";
                CrLedgerCode = "20101002";
                CrLedgerName = "SOD A/C -308";
            }
            else if (model.TransactionMode == "Payment")
            {
                model.DrAmount = model.Amount;
                DrLedgerId = "LL1";
                DrLedgerCode = "20101002";
                DrLedgerName = "SOD A/C -308";

                CrLedgerId = "AL221";
                CrLedgerCode = "10201003";
                CrLedgerName = "Transfer Account";
            }
            ViewBag.AccountInfo = new SelectList(await bankservice.GetBankAccount(), "BankAcInfoId", "AccountNo");

            ViewBag.TransactionType = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Transfer",Value="Transfer"}
             
            };
            ViewBag.TransactionMode = new List<SelectListItem>()
            {
              new SelectListItem() { Text="Deposit",Value="Deposit"},
              new SelectListItem() { Text="Withdraw",Value="Withdraw"},
              new SelectListItem() { Text="Received",Value="Received"},
              new SelectListItem() { Text="Payment",Value="Payment"}
            };

            if (ModelState.IsValid)
            {
                try
                {
                    int ResultID = 0;
                    ResultID= await bankservice.UpdateAsync(model);

                    if (ResultID > 0) 
                    {
                      
                    
                        // Delete
                        var voucherDelete = _db.Vouchers.Where(item => item.TransactionId == ResultID.ToString() && item.EntryFrom == "Bank Deposite & Withdraw").ToList();
                        _db.Vouchers.RemoveRange(voucherDelete);
                        _db.SaveChanges();

                        // Create two entities
                        var DrVoucher = new Voucher
                        {

                            MasterNo = model.AccountId.ToString(),
                            FiscalYearId = fsl,
                            VoucherNo = VoucherNo,
                            VoucherDate = model.TransactionDate,
                            ChequeNo = "",
                            ChequeDate = model.TransactionDate.ToString(),
                            VoucherType = "jau",
                            LedgerId = DrLedgerId,
                            LedgerCode = DrLedgerCode,
                            LedgerName = DrLedgerName,
                            BalanceAmount = 0,
                            DrAmount = model.Amount,
                            CrAmount = 0,
                            Narration = model.Particulars,
                            TransactionWith = model.TransactionMode,
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
                            EntryFrom = "Bank Deposite & Withdraw",
                            UserName = user.FullName,
                            UserIp = GetIPV4,
                            EntryTime = DateTime.Now,
                            TransactionId = ResultID.ToString()

                        };

                        var CrVoucher = new Voucher
                        {

                            MasterNo = model.AccountId.ToString(),
                            FiscalYearId = fsl,
                            VoucherNo = VoucherNo,
                            VoucherDate = model.TransactionDate,
                            ChequeNo = "",
                            ChequeDate = model.TransactionDate.ToString(),
                            VoucherType = "jau",
                            LedgerId = CrLedgerId,
                            LedgerCode = CrLedgerCode,
                            LedgerName = CrLedgerName,
                            BalanceAmount = 0,
                            DrAmount = 0,
                            CrAmount = model.Amount,
                            Narration = model.Particulars,
                            TransactionWith = model.TransactionMode,
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
                            EntryFrom = "Bank Deposite & Withdraw",
                            UserName = user.FullName,
                            UserIp = GetIPV4,
                            EntryTime = DateTime.Now,
                            TransactionId = ResultID.ToString()

                        };

                        // Add them to the DbContext
                        _db.Vouchers.Add(DrVoucher);
                        _db.Vouchers.Add(CrVoucher);

                        // Save changes to the database
                        await _db.SaveChangesAsync();


                    }


                    }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await bankservice.BankAccountExistsAsync(model.Id))
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
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult>  GetBankBranchName(int accountId)
        { 
             
         var accountInfo= await bankservice.GetBankBranchName(accountId);

            return Json( new { bankname= accountInfo.bankName.BankNames,branchname=accountInfo.branchInformation.BranchName,
            bankbranch=accountInfo.bankBranch.BankBranchName,accounttype=accountInfo.AccountTypeName });
        }

    }
}
