using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProvidentFund.Data;

namespace ESL.Areas.Accounts.Controllers
{
    public class PrimaryGroupController : Controller
    {
       // private readonly IUnitAccounts _unitAccounts;
        private readonly ILogger<PrimaryGroupController> _logger;
       // private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private AppDbContext _db;
        String sqlCon = "";
       // PrimaryGroupService service;
       // private commonService commonService;
        public PrimaryGroupController(
            //IUnitAccounts unitAccounts, 
            IHttpContextAccessor accessor,
            ILogger<PrimaryGroupController>  logger,
			//IUnitOfWork unitOfWork,
			AppDbContext db
            )
        {
            //_unitAccounts = unitAccounts;
           // _unitOfWork = unitOfWork;
            _accessor = accessor;
            _logger = logger;
            _db = db;
            //service = new PrimaryGroupService(_unitOfWork, _unitAccounts, _accessor,_db);
            //commonService = new commonService(_unitOfWork, _unitAccounts, _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
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
        //public IActionResult primarySave(PrimaryGroup obj)
        //{
        //    bool isUpdate = false;
        //    string msg = "Unable to save!";
        //    if (!string.IsNullOrEmpty(obj.Code) && !string.IsNullOrEmpty(obj.Name))
        //    {
        //        obj.UserIp = SD.getIp();
        //        obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //        var objUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(x=>x.Id==obj.UserId);

        //        if (objUser!=null)
        //        {
        //            obj.UserName = objUser.Name;
        //        }
        //        obj.EntryTime = DateTime.Now;
        //        obj.CompanyId = Convert.ToInt32(commonService.getSession().GetString("companyId"));
                
        //        if (obj.Id == 0)
        //        {
        //            msg = "Information save successfully!";
        //            _unitAccounts.PrimaryGroup.Add(obj);
        //        }
        //        else
        //        {
        //            isUpdate = true;
        //            msg = "Information update successfully!";
        //            _unitAccounts.PrimaryGroup.Update(obj);
        //        }
        //        _unitAccounts.Save();
        //        return Json(new { success=true,isUpdate=isUpdate,message=msg});
        //    }
        //    return Json(new { success = false, isUpdate = isUpdate, message = msg });
        //}
        //public IActionResult getMaxPrimaryCode(string group, string code)
        //{
        //    _logger.LogInformation(" Group: "+group+" code: "+ code);
        //    string maxId = service.maxPCode(group,code);
        //    if (maxId == "0")
        //    {
        //        return Json(new { maxData = "0" });
        //    }
        //    return Json(new { maxData = maxId });
        //}
        //public IActionResult getMax(string id)
        //{
        //    _logger.LogInformation(id);
        //    string maxId =id+ service.max(id);
        //    if(maxId=="0")
        //    {
        //        return Json(new { maxData="0"});
        //    }
        //    return Json(new { maxData = maxId});
        //}
        //public IActionResult nameCheck(string name)
        //{
        //    var obj = _unitAccounts.PrimaryGroup.GetFirstOrDefault(x => x.Name.Equals(name));

        //    if (obj != null)
        //    {
        //        return Json(new { isFind = true });
        //    }
        //    return Json(new { isFind = false });
       // }
        //public IActionResult findData(int id)
        //{
        //    var obj = _unitAccounts.PrimaryGroup.GetFirstOrDefault(x=>x.Id==id);
        //    if (obj != null)
        //    {
        //        return Json(new { data=obj,isFind=true});
        //    }
        //    return Json(new { isFind=false});
        //}
        #region API CALLS
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var allObj = _unitAccounts.PrimaryGroup.GetAll();
        //    return Json(new { data = allObj });
        //}
        #endregion
    }
}
