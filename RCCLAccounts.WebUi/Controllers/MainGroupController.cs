using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

    public class MainGroupController : Controller
    {

        private readonly ILogger<MainGroupController> _logger;
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        String sqlCon = "";
        MainGroupServiceWebUi service;
        private commonService commonService;
        private UserManager<ApplicationUser> _userManager;
        public MainGroupController(
            IHttpContextAccessor accessor,
            ILogger<MainGroupController> logger,
            AppDbContext db,
            UserManager<ApplicationUser> userManager
            )
        {
    
            _accessor = accessor;
            _logger = logger;
            _db = db;
            service = new MainGroupServiceWebUi (_accessor, _db);
            commonService = new commonService( _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> mainSave(MainGroup obj)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if (!string.IsNullOrEmpty(obj.MainGroupId) 
                && !string.IsNullOrEmpty(obj.MainGroupCode)
                && !string.IsNullOrEmpty(obj.MainGroupName)
                && !string.IsNullOrEmpty(obj.PrimaryGroupId)
                )
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

                var primaryGroup = await _db.PrimaryGroups.FirstOrDefaultAsync(x => x.PrimaryGroupId == obj.PrimaryGroupId);

                if (obj.MainId == 0)
                {
                    var MainGroupCreate = new MainGroup
                    {
                        PrimaryId = primaryGroup.PrimaryId,
                        PrimaryGroupId = obj.PrimaryGroupId,
                        PrimaryGroupName = primaryGroup.PrimaryGroupName,

                        MainGroupId = obj.MainGroupId,
                        MainGroupName = obj.MainGroupName,
                        MainGroupCode =obj.MainGroupCode,
                        Active = obj.Active,
                        EntryFrom = obj.EntryFrom,
                        CompanyId = obj.CompanyId,
                        UserName = obj.UserName,
                        UserIp = obj.UserIp,
                        EntryTime = obj.EntryTime


                    };

                    _db.MainGroups.Add(MainGroupCreate);
                    // Save changes to the database
                    await _db.SaveChangesAsync();
                   
                    msg = "Information save successfully!";
              
                }
                else
                {
                    isUpdate = true;
                    var existingMainGroup = await _db.MainGroups.FindAsync(obj.MainId);
                    if (existingMainGroup != null)
                    {
                        // Update existing entity's properties with new values
                        existingMainGroup.PrimaryGroupName = primaryGroup.PrimaryGroupName;
                        existingMainGroup.MainGroupName = obj.MainGroupName;
                        existingMainGroup.Active = obj.Active;

                        existingMainGroup.UserName = obj.UserName;
                        existingMainGroup.UserIp = obj.UserIp;
                        existingMainGroup.EntryTime = obj.EntryTime;

                        _db.MainGroups.Update(existingMainGroup);
                        await _db.SaveChangesAsync();
                        msg = "Information updated successfully!";
                        isUpdate = true;
                    }
                    else
                    {
                        msg = "Main Group not found!";
                        return Json(new { success = false, isUpdate = isUpdate, message = msg });
                    }
                }

                return Json(new { success = true, isUpdate = isUpdate, message = msg });
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
        public IActionResult getMaxCode(string group, string code, string type)
        {
            if (group == "" || group == "0")
            {
                return Json(new { maxData = "0" });
            }
            string maxId = service.maxCode(group, code, type);
            return Json(new { maxData = maxId });
        }
        public async Task<IActionResult> getPrimaryGroup()
        {
            //var obj = from x in _unitAccounts.PrimaryGroup.GetAll()
            //          select new { Id = x.PrimaryGroupId, Name = x.PrimaryGroupId + "-" + x.Code + "-" + x.Name };

            var obj = await _db.PrimaryGroups
                   .OrderBy(x => x.PrimaryGroupName)
                   .Select(x => new { Id = x.PrimaryGroupId, Name = x.PrimaryGroupId + "-" + x.PrimaryGroupCode + "-" + x.PrimaryGroupName })
                   .ToListAsync();

            return Json(new SelectList(obj,"Id","Name"));
        }
        public IActionResult getMaxId()
        {
            string maxId = "MG" + service.maxId();
            return Json(new { maxData = maxId });
        }
        public async Task<IActionResult> nameCheck(string name)
        {
            //  var obj = _unitAccounts.MainGroup.GetFirstOrDefault(x=>x.MainGroupName.Equals(name));

            var obj = await _db.MainGroups.FirstOrDefaultAsync(x => x.MainGroupName == name);

            if (obj!=null)
            {
                return Json(new { isFind=true});
            }
            return Json(new { isFind = false });
        }
        public IActionResult findData(int id)
        {
            var obj = service.findData(id);
            if (obj != null)
            {
                return Json(new { data = obj, isFind = true });
            }
            return Json(new { isFind = false });
        }
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string type)
        {

            return Json(new { data = service.getAllData(type) });
        }
        #endregion
    }
}
