
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using RCCLAccounts.Data;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RCCLAccounts.WebUi.Controllers
{
 
    public class FiscalYearCloseingController : Controller
    {
     
        private readonly ILogger<FiscalYearCloseingController> _logger;
        private AppDbContext _db { get; }
		private UserManager<ApplicationUser> _userManager;
		private IHttpContextAccessor _accessor;
        private commonService commonService;
        public FiscalYearCloseingController(
            IHttpContextAccessor accessor,
			UserManager<ApplicationUser> userManager,
			ILogger<FiscalYearCloseingController> logger,       
			 AppDbContext db)
        {   
            _accessor = accessor;
            _userManager = userManager;
            _logger = logger;

            _db = db;
            commonService = new commonService( _accessor, _db);
        }

        public IActionResult Index()
        {       
            return View();    
        }

        public IActionResult GetFiscaleYearDate()
        {
            var obj = commonService.getFiscalYearDate();
            return Json(obj);
        }

		public IActionResult GetCurrentFiscaYearRunning()
		{
			bool isRunning = commonService.getCurrentFiscaYearRunning();
			return Json(new { isRunning });
		}

		public IActionResult GetPreviousFiscaYearClose()
		{
			bool IsClosed = commonService.getPreviousFiscaYearClose();
			return Json(new { IsClosed });
		}


		/*  public IActionResult Upsert(int? id)
          {
              FiscalYearInfo fiscalYearInfo = new FiscalYearInfo();

              *//* List<SubGroup> list = _unitAccounts.SubGroup.GetAll().ToList();
               ViewBag.subGroup = new SelectList(list, "Id", "Narration");*//*

              if (id == null)
              {
                  return View(fiscalYearInfo);
              }

              NarrationInfo obj = _unitAccounts.NarrationInfo.Get(id.GetValueOrDefault());

              if (fiscalYearInfo == null)
              {
                  return NotFound();
              }
              return View(fiscalYearInfo);
          }*/
	}
}
        

        /*[BindProperty]
        public SubGroup obj { get; set; }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SubGroup obj)
        {
            string msg = "Unable to save!";
            bool isUpdate = false;
            if (ModelState.IsValid)
            {
                obj.UserIp = SD.getIp();
                obj.UserId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                obj.UserName = _accessor.HttpContext.User.Identity.Name;
                obj.EntryTime = DateTime.Now;
                if (obj.Id == 0)
                {
                    msg = "Information save successfully!";
                    SubGroup Pm = _unitAccounts.SubGroup.Add(obj);
                    _unitAccounts.Save();
                   UpdateTracking(Pm,"New");

                }
                else
                {

                   if (UpdateTracking(obj,"Update"))
                    {
                        isUpdate = true;
                        msg = "Information update successfully!";
                        _unitAccounts.SubGroup.Update(obj);
                        _unitAccounts.Save();
                    }
                    
                }
                // return RedirectToAction(nameof(Index));
                return Json(new { isValid = true, message = msg, update = isUpdate });
            }
            else
            {
                if (obj.Id != 0)
                {
                    obj.EntryTime = DateTime.Now;
                    obj = _unitAccounts.SubGroup.Get(obj.Id);
                }
            }
            //  return View(obj);
            return Json(new { isValid = false, message = msg, update = isUpdate });
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitAccounts.SubGroup.GetAll();
            return Json(new { data = allObj });
        }

        public string isAnyCodeExist()
        {
            int maxCode = _unitAccounts.SubGroup.NumericMax("tbSubGroup", "Code");
            if (maxCode == 1)
            {
                return "Not Exist";
            }
            else
            {
                return "Exist";
            }
        }

        private string getMaxCode()
        {
            return "" + _unitAccounts.SubGroup.NumericMax("tbSubGroup", "Code");
        }


        public Boolean UpdateTracking(SubGroup primaryObject,String Flag)
        {
            return true;
        }

        #endregion;
    }
}
*/