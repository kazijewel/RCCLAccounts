
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
using System.Net;
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
			bool isClosed = commonService.getPreviousFiscaYearClose();
			return Json(new { isClosed });
		}

		[HttpPost]
		public async Task<IActionResult> FirstStepAction()
		{
			try
			{

				var addlist = Dns.GetHostEntry(Dns.GetHostName());
				string GetHostName = addlist.HostName.ToString();
				string GetIPV6 = addlist.AddressList[0].ToString();
				string GetIPV4 = addlist.AddressList[1].ToString();
				var user = await _userManager.GetUserAsync(User);

				string companyID = "B-1";
				string userName = user.FullName.ToString();
				string userIp = GetIPV4;

				commonService.FiscalYearClosingFirstStep(companyID, userName, userIp);

	
				return Json(new { success = true, message = "New fiscal year created successfully." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error to create table: " + ex.Message });
			}
		}


		[HttpPost]
		public async Task<IActionResult> FinalStepAction()
		{
			try
			{

				string companyID = "B-1";
				
				commonService.FiscalYearClosingSecondStep(companyID);

				return Json(new { success = true, message = "Balance Transfer Process completed successfully." });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error to transfer: " + ex.Message });
			}
		}


	}
}
        
