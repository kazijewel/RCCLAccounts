
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RCCLAccounts.WebUi.Controllers
{

    public class NarrationInfoController : Controller
    {

      //  private readonly IUnitAccounts _unitAccounts;
      //  private readonly ILogger<NarrationInfoController> _logger;
       // private IBackgroundTaskQueue _backgroundTaskQueue;
        private AppDbContext _db { get; }

        private IHttpContextAccessor _accessor;
        NarrationServiceWebUi service;
        private UserManager<ApplicationUser> _userManager;
        public NarrationInfoController(
            IHttpContextAccessor accessor,
           // ILogger<NarrationInfoController> logger,

             AppDbContext db,
             UserManager<ApplicationUser> userManager)
        {
          
            _accessor = accessor;
            //  _logger = logger;
            _db = db;
            service = new NarrationServiceWebUi(_accessor, _db);
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            //Menu menu = UserAccessor.getMyActiveMenu(_userManager, _backgroundTaskQueue, User);
            //ViewBag.isAdd = menu.IsAdd;
            //ViewBag.isEdit = menu.IsEdit;
            return View();
        }

        public async Task<IActionResult> narrationSave(Narration obj)
        {
            bool isUpdate = false;
            string msg = "Unable to save!";
            if (!string.IsNullOrEmpty(obj.NarrationCode)
                && !string.IsNullOrEmpty(obj.NarrationName)
                && !string.IsNullOrEmpty(obj.VoucherType)
                
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



                if (obj.NarrationId == 0)
                {
                    var NarrationCreate = new Narration
                    {
                        NarrationCode = obj.NarrationCode,
                        NarrationName = obj.NarrationName,
                        VoucherType = obj.VoucherType,
                        Active = obj.Active,                    
                        CompanyId = obj.CompanyId,
                        UserName = obj.UserName,
                        UserIp = obj.UserIp,
                        EntryTime = obj.EntryTime


                    };

                    _db.Narrations.Add(NarrationCreate);
                    // Save changes to the database
                    await _db.SaveChangesAsync();

                    msg = "Information save successfully!";

                }
                else
                {
                    isUpdate = true;
                    var existingNarration= await _db.Narrations.FindAsync(obj.NarrationId);
                    if (existingNarration != null)
                    {
                        // Update existing entity's properties with new values
                        existingNarration.NarrationName = obj.NarrationName;
                  
                        existingNarration.Active = obj.Active;

                        existingNarration.UserName = obj.UserName;
                        existingNarration.UserIp = obj.UserIp;
                        existingNarration.EntryTime = obj.EntryTime;

                        _db.Narrations.Update(existingNarration);
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
        public IActionResult getMaxId()
        {
            string maxId = "NR" + service.maxId();
            return Json(new { maxData = maxId });
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
        public IActionResult GetAll()
        {

            return Json(new { data = service.getAllData() });
        }
        #endregion
    }
}      