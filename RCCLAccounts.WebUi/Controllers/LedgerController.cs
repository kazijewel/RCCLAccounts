using ESL.DataAccess;
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models.MAccount;
using ESL.Models.ViewModels;
using ESL.Services;
using ESL.TreeMenu;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESL.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    [Authorize]
    public class LedgerController : Controller
    {

        private readonly IUnitAccounts _unitAccounts;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LedgerController> _logger;
        private IBackgroundTaskQueue _backgroundTaskQueue;
        private ApplicationDbContext _db;
        private IHttpContextAccessor _accessor;
        LedgerServiceWebUi service;
        private commonService commonService;
        string sqlCon = "";
        public LedgerController(IUnitAccounts unitAccounts,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor,
            UserManager<IdentityUser> userManager,
            ILogger<LedgerController> logger,
            IBackgroundTaskQueue backgroundTaskQueue,
             ApplicationDbContext db)
        {
            _unitAccounts = unitAccounts;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _userManager = userManager;
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
            _db = db;
            service = new LedgerServiceWebUi(_unitOfWork, _unitAccounts, _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            commonService = new commonService(_unitOfWork, _unitAccounts, _accessor, _db);
        }
        public IActionResult Index()
        {
            Menu menu = UserAccessor.getMyActiveMenu(_userManager, _backgroundTaskQueue, User);
            /*ViewBag.isAdd = menu.IsAdd;
            ViewBag.isEdit = menu.IsEdit;*/
            return View();
        }
        public IActionResult ledgerSave(Ledger obj,decimal debit,decimal credit,int openingId)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if (isValidModel(obj))
            {

                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var objUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == obj.UserId);
                obj.EntryTime = DateTime.Now;
                if (objUser != null)
                {
                    obj.UserName = objUser.Name;
                }
               


                if (obj.Id == 0)
                {
                    msg = "Information save successfully!";
                    ISession session = commonService.getSession();
                    var companyId = session.GetString("companyId");
                    var branchId = session.GetString("branchId");
                    obj.CompanyId = Convert.ToInt32(companyId);
                    //obj.BranchId = Convert.ToInt32(branchId);
                    obj = _unitAccounts.Ledger.Add(obj);
                }
                else
                {
                    isUpdate = true;
                    msg = "Information update successfully!";
                    _unitAccounts.Ledger.Update(obj);
                }
                _unitAccounts.Save();
                int opening = service.openingBalanceSave(obj,debit,credit,openingId);
               
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
        public IActionResult isMainGroup(string id)
        {
            var obj = _unitAccounts.MainGroup.GetFirstOrDefault(x=>x.PrimaryGroupId==id);
            if(obj==null)
            {
                return Json(new { isFind=false});
            }
            return Json(new { isFind = true });
        }
        public IActionResult isSubGroup(string pId,string mId)
        {
            var obj = _unitAccounts.SubGroup.GetFirstOrDefault(x=>x.PrimaryGroupId==pId && x.MainGroupId==mId);
            if(obj==null)
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
        public IActionResult getPrimaryGroup()
        {
            var obj = service.primaryGroup();
            if(obj!=null)
                return Json(new SelectList(obj,"Id","Name"));
            return Json(new {});
        }
        public IActionResult getMainGroup(string pId)
        {
            var obj = service.mainGroup(pId);
            if (obj != null)
                return Json(new SelectList(obj, "Id", "Name"));
            return Json(new { });
        }
        public IActionResult getSubGroup(string pId,string mId)
        {
            var obj = service.subGroup(pId,mId);
            if (obj != null)
                return Json(new SelectList(obj, "Id", "Name"));
            return Json(new { });
        }
    }
}
