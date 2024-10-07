using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ESL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;

namespace RCCLAccounts.WebUi.Controllers
{
   
    public class SubGroupController : Controller
    {
      
        private readonly ILogger<SubGroupController> _logger;
      
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        string sqlCon = "";
        SubGroupServiceWebUi service;
        private commonService commonService;
        private UserManager<ApplicationUser> _userManager;
        public SubGroupController(
            IHttpContextAccessor accessor,
            ILogger<SubGroupController> logger,
            AppDbContext db,
            UserManager<ApplicationUser> userManager
            )
        {
            
            _accessor = accessor;
            _logger = logger;
            _db = db;
            service = new SubGroupServiceWebUi( _accessor, _db);
            commonService = new commonService( _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> subSave(SubGroup obj)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if (!string.IsNullOrEmpty(obj.SubGroupId) 
                && !string.IsNullOrEmpty(obj.SubGroupCode)
                && !string.IsNullOrEmpty(obj.SubGroupName)
                && !string.IsNullOrEmpty(obj.PrimaryGroupId)
                && !string.IsNullOrEmpty(obj.MainGroupId)

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
                var mainGroup = await _db.MainGroups.FirstOrDefaultAsync(x => x.MainGroupId == obj.MainGroupId);

                if (obj.SubId == 0)
                {
                    var SubGroupCreate = new SubGroup
                    {
                        PrimaryId = primaryGroup.PrimaryId,
                        PrimaryGroupId = obj.PrimaryGroupId,
                        PrimaryGroupName = primaryGroup.PrimaryGroupName,

                        MainId = mainGroup.MainId,
                        MainGroupId = obj.MainGroupId,
                        MainGroupName = mainGroup.MainGroupName,

                        SubGroupId = obj.SubGroupId,
                        SubGroupName = obj.SubGroupName,
                        SubGroupCode = obj.SubGroupCode,
                        Active = obj.Active,
                        EntryFrom = obj.EntryFrom,
                        CompanyId = obj.CompanyId,
                        UserName = obj.UserName,
                        UserIp = obj.UserIp,
                        EntryTime = obj.EntryTime


                    };

                    _db.SubGroups.Add(SubGroupCreate);
                    // Save changes to the database
                    await _db.SaveChangesAsync();
                    msg = "Information save successfully!";
                    
                }
                else
                {
                    isUpdate = true;
                    var existingSubGroup = await _db.SubGroups.FindAsync(obj.SubId);

                    if (existingSubGroup != null)
                    {
                        // Update existing entity's properties with new values
                        existingSubGroup.PrimaryGroupName = primaryGroup.PrimaryGroupName;
                        existingSubGroup.MainGroupName =  mainGroup.MainGroupName;

                        existingSubGroup.SubGroupName = obj.SubGroupName;
                        existingSubGroup.SubGroupCode = obj.SubGroupCode;

                        existingSubGroup.Active = obj.Active;

                        existingSubGroup.UserName = obj.UserName;
                        existingSubGroup.UserIp = obj.UserIp;
                        existingSubGroup.EntryTime = obj.EntryTime;

                        _db.SubGroups.Update(existingSubGroup);
                        await _db.SaveChangesAsync();
                        msg = "Information updated successfully!";
                        isUpdate = true;
                    }
                    else
                    {
                        msg = "Sub Group not found!";
                        return Json(new { success = false, isUpdate = isUpdate, message = msg });
                    }

                }

                return Json(new { success = true, isUpdate = isUpdate, message = msg });
            }
            return Json(new { success = false, isUpdate = isUpdate, message = msg });
        }
        public IActionResult getMaxCode(string group, string code, string type)
        {
            if(group=="" || group=="0")
            {
                return Json(new { maxData = "0" });
            }
            string maxId = service.maxCode(group, code, type);
            return Json(new { maxData = maxId });
        }
        
        public async Task<IActionResult> getPrimaryGroup()
        {
            //var obj = from x in _unitAccounts.PrimaryGroup.GetAll()
            //          select new { Id = x.PrimaryGroupId, Name = x.Code + "-" + x.Name };
            var obj = await _db.PrimaryGroups
                  .Select(x => new { Id = x.PrimaryGroupId, Name = x.PrimaryGroupCode + "-" + x.PrimaryGroupName })
                  .ToListAsync();

            return Json(new SelectList(obj,"Id","Name"));
        } 
        public async Task<IActionResult> getMainGroup(string id)
        {
            //var obj = from x in _unitAccounts.MainGroup.GetAll(x=>x.PrimaryGroupId == id)
            //          select new { Id = x.MainGroupId, Name =x.MainGroupCode + "-" + x.MainGroupName };
            var obj = await _db.MainGroups
                     .Where(x => x.PrimaryGroupId == id)  
                     .Select(x => new { Id = x.MainGroupId, Name = x.MainGroupCode + "-" + x.MainGroupName })
                     .ToListAsync();

            return Json(new SelectList(obj,"Id","Name"));
        }
        public async Task<IActionResult> getPrimaryGroupId(string id)
        {
            // var obj = _unitAccounts.PrimaryGroup.GetFirstOrDefault(x => x.PrimaryGroupId == id);
            var obj = await _db.PrimaryGroups.FirstOrDefaultAsync(x => x.PrimaryGroupId == id);
            return Json(obj);
        }
        public IActionResult getMaxId()
        {
            string maxId = "SG" + service.maxId();
            return Json(new { maxData = maxId });
        }
        public async Task<IActionResult> nameCheck(string name)
        {
           // var obj = _unitAccounts.SubGroup.GetFirstOrDefault(x=>x.SubGroupName.Equals(name));

            var obj = await _db.SubGroups.FirstOrDefaultAsync(x => x.SubGroupName == name);

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
