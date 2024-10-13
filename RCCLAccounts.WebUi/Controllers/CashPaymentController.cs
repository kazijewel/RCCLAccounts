using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
using Newtonsoft.Json;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Services;

namespace RCCLAccounts.WebUi.Controllers
{
 
    public class CashPaymentController : Controller
    {

        private readonly ILogger<CashPaymentController> _logger;
        private IHttpContextAccessor _accessor;
        private readonly IWebHostEnvironment _hostEnvironment;  
        private UserManager<ApplicationUser> _userManager;
        private AppDbContext _db;
        string sqlCon = "";
        CashPaymentServiceWebUi service;
        private commonService commonService;
        public FileUpload _fileUpload;
        public CashPaymentController(
            IHttpContextAccessor accessor,
            ILogger<CashPaymentController> logger,
            AppDbContext db,
            IWebHostEnvironment hostEnvironment,
               UserManager<ApplicationUser> userManager
            )
        {
            _hostEnvironment = hostEnvironment;          
            _accessor = accessor;
            _logger = logger;
            _db = db;
            _userManager = userManager;
       
            service = new CashPaymentServiceWebUi (_accessor, _db);
            commonService = new commonService(_accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult>  UniqueLedgerId()
        {
            return await service.getLedgerUniqueId() != "" ? Json(new { data = service.getLedgerUniqueId() }) : Json(new { });
        }
        public async Task<IActionResult> LedgerInfo()
        {
            var obj = await service.ledgerList();
            if(obj!=null)
            {
                //  return Json(new SelectList(obj,"Id","Name"));
                return Json(new SelectList((IEnumerable<dynamic>)obj.Value, "Id", "Name"));
            }
            return Json(new { });
        }
        public async Task<IActionResult> LedgerInfoCash()
        {
            var obj = await service.ledgerListCash();
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
        public async Task<IActionResult>  GetLedgerId(string id)
        {
            string ledgerId = await service.getLedgerId(id);
            if (ledgerId != null)
            {
                return Json(new { ledgerId= ledgerId });
            }
            return Json(new { });
        }
        public IActionResult GetCashPaymentAll(string fromDate,string toDate)
        {
            var obj = service.getAllData(fromDate,toDate);
            if(obj!=null)
            {
                return Json(new { data=obj});
            }
            return Json(new { });
        }
        
        public IActionResult TransactionId(string date)
        {
            var obj = commonService.getTransactionId();
            if(obj!=null)
            {
                return Json(new { data=obj});
            }
            return Json(new { });
        }
        
        public IActionResult VoucherNo(string date)
        {
            var obj = service.getVoucherNo(date);
            if(obj!=null)
            {
                return Json(new { data=obj});
            }
            return Json(new { });
        }
        
        public IActionResult findData(string id)
        {
            var obj = service.getData(id);
            if(obj!=null)
            {
                return Json(new { data=obj,isFind=true});
            }
            return Json(new {isFind=false });
        }

        public IActionResult Narrations()
        {
            var obj = commonService.getNarrations();
            return Json(new { data = obj });
        }

        //public IActionResult FileRemove()
        //{
        //    SD.FileRemove();
        //    return Json(new { data = true });
        //}

        [HttpPost]
        public async Task<IActionResult> cashPaymentSave( 
            //IFormCollection data
            List<Voucher> objList, 
            string cashHeadId, 
            string cashHead
            )
        {

            bool isUpdate = false;
            string msg = "Unable to save!";
            //var responseDTO = data["responseDTO"];
            //ResponseDTO obj = JsonConvert.DeserializeObject<ResponseDTO>(responseDTO);
            if(objList != null && objList.Count>0)
            {
                //Console.WriteLine(obj.Vouchers[0].LedgerName);

                //Console.WriteLine(Request.Form["responseDTO"]);
               // _logger.LogInformation(Request.Form.Files[0].FileName);
                if (isValidModel(objList))
                {
                    var addlist = Dns.GetHostEntry(Dns.GetHostName());
                    string GetHostName = addlist.HostName.ToString();
                    string GetIPV6 = addlist.AddressList[0].ToString();
                    string GetIPV4 = addlist.AddressList[1].ToString();
                    var user = await _userManager.GetUserAsync(User);

                    string userIp = "";
  
                    string userName = user.FullName.ToString();
                    userIp = GetIPV4;
                    
                    objList[0].AttachBill = setAttachment(objList[0].AutoId, objList[0].AttachBill);
                    
                    int cashPayment = service.cashSave(objList, cashHeadId, cashHead,userName, userIp);
                    if (cashPayment > 0)
                    {
                        return Json(new { success = true, isUpdate = isUpdate, message = "Information save successfully!" });
                    }
                    else
                    {
                        msg = "Unable to save! Please check fiscale year date or something worng. please try again.";
                    }
                }
            }

            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
        public bool isValidModel(List<Voucher> obj)
        {
            bool ret = false;
            if (
                    obj[0].LedgerId != "" && obj[0].LedgerName != "" && obj[0].DrAmount != 0 && obj[0].VoucherNo != ""
                    && obj[0].VoucherDate != null
                )
            {
                ret = true;
            }
            return ret;
        }

        private string setAttachment(long Id,string Attachment)
        {
            _fileUpload = new FileUpload(_hostEnvironment, _accessor);

            string fileName = "CPV-" + DateTime.Now.Ticks.ToString();

            string files = _fileUpload.getUploadUrl(Attachment,
                   @"images\accounts\", @"\images\accounts\", fileName,"");

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
