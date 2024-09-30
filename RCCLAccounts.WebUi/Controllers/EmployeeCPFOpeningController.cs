using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Core.Services;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.WebUi.Models;
using System.Net;
using System.Security.Claims;

namespace RCCLAccounts.WebUi.Controllers
{
    public class EmployeeCPFOpeningController : Controller
    {

        private readonly IEmployeeCPFOpeningService _employeeCPFOpeningService;
        private readonly ILoanInformationService _loanInformationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;
        public EmployeeCPFOpeningController(IEmployeeCPFOpeningService employeeCPFOpeningService,
            ILoanInformationService loanInformationService,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
			_employeeCPFOpeningService = employeeCPFOpeningService;
            _loanInformationService = loanInformationService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var employeeCPFOpening = await _employeeCPFOpeningService.GetAllEmployeeCPFOpeningAsync();
            return View(employeeCPFOpening);
        }

        public async Task<IActionResult>  CreateAsync()
        {
            //ViewBag.Employee = new SelectList(await _loanInformationService.GetAllEmployeeAsync(), "EmpolyeeId", "EmployeeName");

            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();
            ViewBag.IPV4 = GetIPV4;
            ViewBag.IPV6 = GetIPV6;
            string userID= _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.userID = userID;
            //string userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            //ViewBag.userName = userName;

            //var context = new AppContext();
            //var username = User.Identity.Name;

            //if (!string.IsNullOrEmpty(username))
            //{
            //    var user = context.Users.SingleOrDefault(u => u.UserName == username);

            //    ViewBag.userName = user.FullName;
            //}
            var user = await _userManager.GetUserAsync(User);
            var fullname = user.FullName;
            ViewBag.userName = fullname;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateAsync([Bind("Id,EmpolyeeId,OpeningDate,OpOwnDepositeAmt,OpRCCLContributionAmt,OpInterestDistributionAmt,OpRCCLInterestDistributionAmt,UserId,UserName,UserIp,EntryTime")] EmployeeCPFOpeningModel employeeCPFOpening)
        {

           // ViewBag.Employee = new SelectList(await _loanInformationService.GetAllEmployeeAsync(), "EmpolyeeId", "EmployeeName");
            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            employeeCPFOpening.UserIp = GetIPV4;
            employeeCPFOpening.EntryTime = DateTime.Now;

            //string userID = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //employeeCPFOpening.UserId = userID;
            //string userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            //employeeCPFOpening.UserName = userName;


            var user = await _userManager.GetUserAsync(User);       
            employeeCPFOpening.UserId = user.Id.ToString();
            employeeCPFOpening.UserName = user.FullName;

            if (ModelState.IsValid)
            {
                await _employeeCPFOpeningService.CreateAsync(employeeCPFOpening);
                TempData["AlertMessage"] = "CPF Opening Create Successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeCPFOpening);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeCPFOpening = await _employeeCPFOpeningService.GetByIdAsync(id);
            if (employeeCPFOpening == null)
            {
                return NotFound();
            }

            return View(employeeCPFOpening);
        }
        public async Task<IActionResult> Edit(int? id)
        {
         
            if (id == null)
            {
                return NotFound();
            }
            
            var employeeCPFOpening = await _employeeCPFOpeningService.GetByIdAsync(id);
            if (employeeCPFOpening == null)
            {
                return NotFound();
            }

            //ViewBag.Employee = new SelectList(await _loanInformationService.GetAllEmployeeAsync(), "EmpolyeeId", "EmployeeName");
            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            return View(employeeCPFOpening);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,EmpolyeeId,OpeningDate,OpOwnDepositeAmt,OpRCCLContributionAmt,OpInterestDistributionAmt,OpRCCLInterestDistributionAmt,UserId,UserName,UserIp,EntryTime")] EmployeeCPFOpeningModel employeeCPFOpening)
        {
           // ViewBag.Employee = new SelectList(await _loanInformationService.GetAllEmployeeAsync(), "EmpolyeeId", "EmployeeName");
            ViewBag.Employee = new SelectList(await _employeeCPFOpeningService.GetAllEmpNoAndName(), "EmpolyeeId", "EmployeeName");
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            employeeCPFOpening.UserIp = GetIPV4;
            employeeCPFOpening.EntryTime =  DateTime.Now;

            //string userID = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //employeeCPFOpening.UserId = userID;
            //string userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            //employeeCPFOpening.UserName = userName;

            var user = await _userManager.GetUserAsync(User);
            employeeCPFOpening.UserId = user.Id.ToString();
            employeeCPFOpening.UserName = user.FullName;


            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeCPFOpeningService.UpdateAsync(employeeCPFOpening);
                    TempData["AlertMessage"] = "CPF Opening Update Successfully!!!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EmployeeCPFOpeningExistsAsync(employeeCPFOpening.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeCPFOpening);
        }

        private async Task<bool> EmployeeCPFOpeningExistsAsync(int id)
        {
            return await _employeeCPFOpeningService.EmployeeCPFOpeningExistsAsync(id);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DefaultRequest request)
        {
            var employeeCPFOpening = await _employeeCPFOpeningService.GetByIdAsync(request.Id);
            if (employeeCPFOpening != null)
            {
                await _employeeCPFOpeningService.DeleteAsync(request.Id);
            }
            return Ok(new DefaultResponse("Success"));
        }
    }

}
