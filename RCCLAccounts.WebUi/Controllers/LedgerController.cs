
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using RCCLAccounts.WebUi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RCCLAccounts.WebUi.Controllers
{
  
    public class LedgerController : Controller
    {

        
        //private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LedgerController> _logger;
       // private IBackgroundTaskQueue _backgroundTaskQueue;
        private AppDbContext _db;
        private IHttpContextAccessor _accessor;
        LedgerServiceWebUi service;
        private commonService commonService;
        string sqlCon = "";
        private UserManager<ApplicationUser> _userManager;
        public LedgerController(
            IHttpContextAccessor accessor,
            //<IdentityUser> userManager,
            ILogger<LedgerController> logger,
             //IBackgroundTaskQueue backgroundTaskQueue,
             AppDbContext db,
              UserManager<ApplicationUser> userManager)
        {
            
            _accessor = accessor;
            //_userManager = userManager;
            _logger = logger;
           //_backgroundTaskQueue = backgroundTaskQueue;
            _db = db;
            service = new LedgerServiceWebUi(_accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService(_accessor, _db);
            _userManager = userManager;
        }
        public IActionResult Index()
        {
          //  Menu menu = UserAccessor.getMyActiveMenu(_userManager, _backgroundTaskQueue, User);
            /*ViewBag.isAdd = menu.IsAdd;
            ViewBag.isEdit = menu.IsEdit;*/
            return View();
        }
        public async Task<IActionResult> ledgerSave(Ledger obj,decimal debit,decimal credit,int openingId)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            int LedgerAutoId = 0; // AutoId will be used after Ledger insert
            if (isValidModel(obj))
            {

                var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string GetHostName = addlist.HostName.ToString();
                string GetIPV6 = addlist.AddressList[0].ToString();
                string GetIPV4 = addlist.AddressList[1].ToString();
                var user = await _userManager.GetUserAsync(User);
                string UserName = user.FullName.ToString();
                obj.UserIp = GetIPV4;
                obj.UserName = UserName;
                obj.EntryTime = DateTime.Now;

                var primaryGroup = await _db.PrimaryGroups.FirstOrDefaultAsync(x => x.PrimaryGroupId == obj.PrimaryGroupId);
                var mainGroup = await _db.MainGroups.FirstOrDefaultAsync(x => x.MainGroupId == obj.MainGroupId);
                var subGroup = await _db.SubGroups.FirstOrDefaultAsync(x => x.SubGroupId == obj.SubGroupId);

            
                if (obj.AutoId == 0)
                    {
                    
                    msg = "Information save successfully!";

                    var LedgerCreate = new Ledger
                    {
                        OpeningDate = obj.OpeningDate,
                        PrimaryId = primaryGroup.PrimaryId,
                        PrimaryGroupId = obj.PrimaryGroupId,
                        MainId = mainGroup?.MainId ?? null,
                        MainGroupId = mainGroup?.MainGroupId ?? "",
                        SubId = subGroup?.SubId ?? null,
                        SubGroupId = subGroup?.SubGroupId ?? "",
                        LedgerId = obj.LedgerId,
                        LedgerCode = obj.LedgerCode,
                        LedgerName = obj.LedgerName,
                        ParentId = obj.ParentId,
                        CreateFrom = obj.CreateFrom,
                        LedgerType = obj.LedgerType,
                        CreditLimit = 365,
                        Active = obj.Active,
                        CompanyId = obj.CompanyId,
                        EntryFrom = obj.EntryFrom,                    
                        UserName = obj.UserName,
                        UserIp = obj.UserIp,
                        EntryTime = obj.EntryTime


                    };

                    _db.Ledgers.Add(LedgerCreate);
                    // Save changes to the database
                    await _db.SaveChangesAsync();
                    LedgerAutoId = LedgerCreate.AutoId;

                }
                    else
                    {
                        isUpdate = true;
                        
                    var existingLedger = await _db.Ledgers.FindAsync(obj.AutoId);

                    if (existingLedger != null)
                    {
                        // Update existing entity's properties with new values
    
                        existingLedger.LedgerName = obj.LedgerName;
                        existingLedger.LedgerType = obj.LedgerType;
                        existingLedger.OpeningDate = obj.OpeningDate;
                        existingLedger.Active = obj.Active;

                        existingLedger.UserName = obj.UserName;
                        existingLedger.UserIp = obj.UserIp;
                        existingLedger.EntryTime = obj.EntryTime;

                        _db.Ledgers.Update(existingLedger);
                        await _db.SaveChangesAsync();
                        msg = "Information updated successfully!";
                        isUpdate = true;

                        LedgerAutoId = obj.AutoId;
                    }
                    else
                    {
                        msg = "Sub Group not found!";
                        return Json(new { success = false, isUpdate = isUpdate, message = msg });
                    }
                }
                //    _unitAccounts.Save();
                   int opening = service.openingBalanceSave(obj,debit,credit,openingId, LedgerAutoId);

                return Json(new { success = true, isUpdate = isUpdate, message = msg });
                
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
        public bool isValidModel(Ledger obj)
        {
            if (!string.IsNullOrEmpty(obj.LedgerName)
                && obj.OpeningDate != null
                && !string.IsNullOrEmpty(obj.LedgerCode)
                && !string.IsNullOrEmpty(obj.LedgerType)
                && !string.IsNullOrEmpty(obj.PrimaryGroupId)
                && !string.IsNullOrEmpty(obj.CreateFrom)
                && !string.IsNullOrEmpty(obj.ParentId)
                )
            {
                return true;
            }
            return false;
        }
        #region Table_Data_Load
        [HttpGet]
        public IActionResult GetData(string type)
        {
            var obj = service.getAllData(type);
            return Json(new { data = obj});
        }
        #endregion
        public async Task<IActionResult> isMainGroup(string id)
        {
            //var obj = _unitAccounts.MainGroup.GetFirstOrDefault(x=>x.PrimaryGroupId==id);
            var obj = await _db.MainGroups
                   .FirstOrDefaultAsync(x => x.PrimaryGroupId == id);

            if (obj==null)
            {
                return Json(new { isFind=false});
            }
            return Json(new { isFind = true });
        }
        public async Task<IActionResult> isSubGroup(string pId,string mId)
        {
            //var obj = _unitAccounts.SubGroup.GetFirstOrDefault(x=>x.PrimaryGroupId==pId && x.MainGroupId==mId);

            var obj = await _db.SubGroups
                .FirstOrDefaultAsync(x => x.PrimaryGroupId == pId && x.MainGroupId == mId );

            if (obj==null)
            {
                return Json(new { isFind=false});
            }
            return Json(new { isFind = true });
        }
        public IActionResult findData(int id)
        {
            var obj = service.getFirstData(id);
            if (obj != null)
            {
                return Json(new { data = obj, isFind = true });
            }
            return Json(new { isFind = false });
        }
        public IActionResult getPrimaryGroupId(int id)
        {
            string maxId = service.PrimaryGroupId(id);
            if (maxId == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxId });
        }
        public IActionResult getMainGroupId(int id)
        {
            string maxId = service.MainGroupId(id);
            if (maxId == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxId });
        }
        public IActionResult getSubGroupId(int id)
        {
            string maxId = service.SubGroupId(id);
            if (maxId == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxId });
        }
        public IActionResult getMaxId(string pId)
        {
            string maxId = service.maxId(pId);
            if (maxId == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxId });
        }
        public IActionResult getMaxCode(string pId, string mId, string sId)
        {
            var maxCode = service.maxCode(pId,mId,sId);
            if (maxCode == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxCode });
        }
        public async Task<IActionResult> getPrimaryGroup()
        {
            var obj = await service.primaryGroup();
            if(obj!=null && obj.Value != null)            
                 return Json(new SelectList((IEnumerable<dynamic>)obj.Value, "Id", "Name"));
            return Json(new {});
        }
        public async Task<IActionResult> getMainGroup(string pId)
        {
            var obj = await service.mainGroup(pId);
            if (obj != null && obj.Value != null)
                return Json(new SelectList((IEnumerable<dynamic>)obj.Value, "Id", "Name"));
            return Json(new { });
        }


        public async Task<IActionResult> getSubGroup(string pId,string mId)
        {
            var obj = await service.subGroup(pId,mId);
            if (obj != null && obj.Value != null)
                return Json(new SelectList((IEnumerable<dynamic>)obj.Value, "Id", "Name"));
            return Json(new { });
        }

        public IActionResult GetFiscaleYearDate()
        {
            var obj = commonService.getFiscalYearDate();
            return Json(obj);
        }
    }
}
