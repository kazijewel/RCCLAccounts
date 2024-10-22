using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Services;

namespace RCCLAccounts.WebUi.Controllers
{

    public class JournalVoucherController : Controller
    {

        private readonly ILogger<CashPaymentController> _logger;
      
        private IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _hostEnvironment;
       
        private readonly UserManager<ApplicationUser> _userManager;
        private AppDbContext _db;
        string sqlCon = "";
        JournalVoucherServiceWebUi service;
        public FileUpload _fileUpload;
        private commonService commonService;
        public JournalVoucherController(
            IHttpContextAccessor accessor,
            ILogger<CashPaymentController> logger,
            AppDbContext db,
            IWebHostEnvironment hostEnvironment,         
            UserManager<ApplicationUser> userManager
            )
        {         
            _accessor = accessor;
            _logger = logger;
            _db = db;
            _userManager = userManager;
          
            service = new JournalVoucherServiceWebUi( _accessor, _db);
            commonService = new commonService(  _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> LedgerInfo()
        {
            var obj = await service.ledgerList();
            if(obj!=null)
            {
                //return Json(new SelectList(obj,"Id","Name"));
                return Json(new SelectList((IEnumerable<dynamic>)obj.Value, "Id", "Name"));
            }
            return Json(new { });
        }
        public async Task<IActionResult> LedgerInfoDrCr()
        {
            var obj = await service.ledgerListDrCrHead();
            if(obj!=null)
            {
                // return Json(new SelectList(obj,"Id","Name"));
                return Json(new SelectList((IEnumerable<dynamic>)obj.Value, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult LedgerBudgetBalance(int id,string date)
        {
            var obj = service.LedgerBudgetBalance(id,date);
            if(obj!=null)
            {
                return Json(obj);
            }
            return Json(new { });
        }
        public async Task<IActionResult> GetLedgerId(string id)
        {
            string ledgerId = await service.getLedgerId(id);
            if (ledgerId != null)
            {
                return Json(new { ledgerId= ledgerId });
            }
            return Json(new { });
        }
        public IActionResult GetJournalVoucher(string fromDate,string toDate)
        {
            var obj = service.getAllData(fromDate,toDate);
            if(obj!=null)
            {
                return Json(new { data=obj});
            }
            return Json(new { });
        }
        
        public IActionResult findData(string id,string date)
        {
            var obj = service.getData(id,date);
            if(obj!=null)
            {
                return Json(new { data=obj,isFind=true});
            }
            return Json(new {isFind=false });
        }
        public IActionResult GetVoucherNo(string date)
        {
            string obj = service.getVoucherNo(date);
            if (obj != null)
            {
                return Json(new { data = "JV-NO-"+obj });
            }
            return Json(new {  });
        }
        public IActionResult GetTransactionId(string date)
        {
            var obj = commonService.getTransactionId();
            if (obj!=null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }
        public async Task<IActionResult> journalVoucherSave(List<Voucher> objList,
            string bankHeadId,string bankHeadName,
            string voucherType)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";

            if (isValidModel(objList))
            {
               
                //string userIp = SD.getIp();
                //string userName = "";
                //string userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //var objUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userId);
                //if (objUser != null)
                //{
                //    userName = objUser.Name;
                //}

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();
                var user = await _userManager.GetUserAsync(User);

                string userIp = "";

                string userName = user.FullName.ToString();
                userIp = GetIPV4;

                var bankLedger = await _db.LedgerOpeningBalances.FirstOrDefaultAsync(x => x.LedgerId == bankHeadId);

                foreach (var voucher in objList)
                {
                    // Fetch cash ledger for the current voucher's LedgerId
                    var OpsiteLedgers = await _db.LedgerOpeningBalances
                        .FirstOrDefaultAsync(x => x.LedgerId == voucher.LedgerId); // Assuming LedgerId is a property of Voucher

                    if (OpsiteLedgers != null)
                    {
                        // Set properties in the voucher based on the cashLedger if needed
                        voucher.LedgerCode = OpsiteLedgers.LedgerCode; // Example: set the LedgerCode
                                                                       // Add any other properties you need to set from cashLedger
                    }

                }



                objList[0].AttachBill = setAttachment(objList[0].AutoId, objList[0].AttachBill);
                
                int save = service.journalVoucherSave(objList,voucherType, bankHeadId,
                    bankHeadName, bankLedger.LedgerCode, userName, userIp);
               
                if (save > 0)
                {
                    return Json(new { success = true, 
                        message = "Information "+(objList[0].AutoId!=0?"update":"save")+" successfully!" });
                }
                else
                {
                    msg = "Unable to save! Please check fiscale year date or something worng. please try again.";
                }
            }
            return Json(new { success = false, message = msg });
        }
        public bool isValidModel(List<Voucher> obj)
        {
            bool ret = false;
            if (
                    obj[0].LedgerId != "" && obj[0].LedgerName != "" && obj[0].DrAmount != 0 && obj[0].VoucherNo != ""
                )
            {
                ret = true;
            }
            return ret;
        }

        private string setAttachment(long Id, string Attachment)
        {
            _fileUpload = new FileUpload(_hostEnvironment, _accessor);

            string fileName = "JV-" + DateTime.Now.Ticks.ToString();

            string files = _fileUpload.getUploadUrl(Attachment,
                   @"images\accounts\", @"\images\accounts\", fileName, "");

            if (string.IsNullOrEmpty(files))
            {
                if (Id > 0)
                {
                    if (Attachment == "" || Attachment == null)
                    {
                        files = "";
                    }
                    else
                    {
                        files = Attachment;
                    }
                }
                else
                {
                    files = "";
                }

            }

            Attachment = files;
            return Attachment;
        }
    }
}
