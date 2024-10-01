using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Services;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ESL.Areas.Accounts.Controllers
{
    public class PrimaryGroupController : Controller
    {
 
        private readonly ILogger<PrimaryGroupController> _logger;
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        String sqlCon = "";
        PrimaryGroupServiceWebUi service;
        public IPrimaryGroupService primaryGroupService { get; set; }
        private UserManager<ApplicationUser> _userManager;
        public PrimaryGroupController(IPrimaryGroupService _primaryGroupService,
            IHttpContextAccessor accessor,
            ILogger<PrimaryGroupController>  logger,
			AppDbContext db,
             UserManager<ApplicationUser> userManager
            )
        {
            this.primaryGroupService = _primaryGroupService;
            _accessor = accessor;
            _logger = logger;
            _db = db;
            service = new PrimaryGroupServiceWebUi( _accessor,_db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Upsert(int? id)
        //{
        //    PrimaryGroup obj = new PrimaryGroup();
        //    return View(obj);

        //}
        public async Task<IActionResult> primarySave(PrimaryGroup obj)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if (!string.IsNullOrEmpty(obj.PrimaryGroupCode) && !string.IsNullOrEmpty(obj.PrimaryGroupName))
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
                obj.CompanyId = "B-1";

                var PrGroupCreate = new PrimaryGroup
                {
                    PrimaryGroupId=obj.PrimaryGroupId,
                    PrimaryGroupName=obj.PrimaryGroupName,
                    PrimaryGroupCode=obj.PrimaryGroupCode,
                    GroupName=obj.GroupName,
                    NoteNo=obj.NoteNo,
                    Active=1,
                    ItemOf=obj.ItemOf,
                    CompanyId=obj.CompanyId,
                    UserName=obj.UserName,
                    UserIp=obj.UserIp,
                    EntryTime=obj.EntryTime


                };

                if (obj.PrimaryId == 0)
                {
                                
                    _db.PrimaryGroups.Add(PrGroupCreate);            
                    // Save changes to the database
                    await _db.SaveChangesAsync();
                    msg = "Information save successfully!";
                }
                else
                {
                    isUpdate = true;
                    msg = "Information update successfully!";
                  //  _unitAccounts.PrimaryGroup.Update(obj);
                }
                //_unitAccounts.Save();
                return Json(new { success = true, isUpdate = isUpdate, message = msg });
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
        public IActionResult getMaxPrimaryCode(string group, string code)
        {
            _logger.LogInformation(" Group: " + group + " code: " + code);
            string maxId = service.maxPCode(group, code);
            if (maxId == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxId });
        }
        public IActionResult getMax(string id)
        {
            _logger.LogInformation(id);
            string maxId = id + service.max(id);
            if (maxId == "0")
            {
                return Json(new { maxData = "0" });
            }
            return Json(new { maxData = maxId });
        }
        public async Task<IActionResult> nameCheck(string name)
        {
            // var obj = _unitAccounts.PrimaryGroup.GetFirstOrDefault(x => x.Name.Equals(name));
            var obj = await primaryGroupService.GetAllPrimaryGroup();
            if (obj != null)
            {
                return Json(new { isFind = true });
            }
            return Json(new { isFind = false });
        }
        public async Task <IActionResult> findData(int id)
        {
            // var obj = _unitAccounts.PrimaryGroup.GetFirstOrDefault(x => x.Id == id);
            var obj = await primaryGroupService.GetByIdAsync(id);
            if (obj != null)
            {
                return Json(new { data = obj, isFind = true });
            }
            return Json(new { isFind = false });
        }
        #region API CALLS
        [HttpGet]
        public async Task<IActionResult>   GetAll()
        {
            //var allObj = _unitAccounts.PrimaryGroup.GetAll();
            var allObj = await primaryGroupService.GetAllPrimaryGroup();
            return Json(new { data = allObj });
        }
        #endregion
    }
}
