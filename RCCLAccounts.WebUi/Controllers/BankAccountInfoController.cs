using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Core.Services;
using ProvidentFund.Data.Entities;
using ProvidentFund.WebUi.Models;
using System.Collections.Generic;

namespace ProvidentFund.WebUi.Controllers
{
    public class BankAccountInfoController : Controller
    {

        private readonly IBankAccountInfoService _bankAccountInfoService;
        public BankAccountInfoController(IBankAccountInfoService bankAccountInfoService)
        {
            _bankAccountInfoService = bankAccountInfoService;
        }

        public async Task<IActionResult> Index()
        {
            var bankAccountInfo = await _bankAccountInfoService.GetAllBankAccountInfoAsync();
            return View(bankAccountInfo);
        }
        
        public async Task<IActionResult> CreateAsync()
        {
            List<SelectListItem> AccountType = new()
            {
                new SelectListItem { Value = "AC-2", Text = "STD" },
                new SelectListItem { Value = "AC-4", Text = "Monthly Interest" },
                new SelectListItem { Value = "AC-5", Text = "FDR" },
                new SelectListItem { Value = "AC-1", Text = "SOD" },
            };
            
            //assigning SelectListItem to view Bag
            ViewBag.AccountTypes = AccountType;


            ViewBag.Branch = new SelectList(await _bankAccountInfoService.GetAllBranchAsync(), "BranchId", "BranchName");
            ViewBag.Bank = new SelectList(await _bankAccountInfoService.GetAllBankAsync(), "BankId", "BankNames");
            ViewBag.BankBranch = new SelectList(await _bankAccountInfoService.GetAllBankBranchAsync(), "BankBranchId", "BankBranchName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("BranchId,BankId,BankBranchId,AccountTypeId,AccountTypeName,AccountNo,AccountName,OpeningDate,DurationMonth,RateOfInterest")] BankAccountInfoModel bankAccountInfo)
        {
            List<SelectListItem> AccountType = new()
            {
                new SelectListItem { Value = "AC-2", Text = "STD" },
                new SelectListItem { Value = "AC-4", Text = "Monthly Interest" },
                new SelectListItem { Value = "AC-5", Text = "FDR" },
                new SelectListItem { Value = "AC-1", Text = "SOD" },
            };

            //assigning SelectListItem to view Bag
            ViewBag.AccountTypes = AccountType;


            ViewBag.Branch = new SelectList(await _bankAccountInfoService.GetAllBranchAsync(), "BranchId", "BranchName");
            ViewBag.Bank = new SelectList(await _bankAccountInfoService.GetAllBankAsync(), "BankId", "BankNames");
            ViewBag.BankBranch = new SelectList(await _bankAccountInfoService.GetAllBankBranchAsync(), "BankBranchId", "BankBranchName");

            DateTime ct = bankAccountInfo.OpeningDate;
            DateTime future = ct.AddMonths(bankAccountInfo.DurationMonth);
            bankAccountInfo.ExpiryDate = future;
            if (bankAccountInfo.AccountTypeId == "AC-2")
            {
                bankAccountInfo.AccountTypeName = "STD";
            }
            else if (bankAccountInfo.AccountTypeId == "AC-4")
            {
                bankAccountInfo.AccountTypeName = "Monthly Interest";
            }
            else if (bankAccountInfo.AccountTypeId == "AC-5")
            {
                bankAccountInfo.AccountTypeName = "FDR";
            }
            else if (bankAccountInfo.AccountTypeId == "AC-1")
            {
                bankAccountInfo.AccountTypeName = "SOD";
            }
            //bankAccountInfo.ExpiryDate = DateTime.Now;
            bankAccountInfo.AccountStatus = "Active";

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();

            if (ModelState.IsValid)
            {
                await _bankAccountInfoService.CreateAsync(bankAccountInfo);
                TempData["AlertMessage"] = "Bank Information Create Successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(bankAccountInfo);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccountInfo = await _bankAccountInfoService.GetByIdAsync(id);
          

            if (bankAccountInfo == null)
            {
                return NotFound();
            }

            return View(bankAccountInfo);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankAccountInfo = await _bankAccountInfoService.GetByIdAsync(id);
            List<SelectListItem> AccountType = new()
            {
                new SelectListItem { Value = "AC-2", Text = "STD" },
                new SelectListItem { Value = "AC-4", Text = "Monthly Interest" },
                new SelectListItem { Value = "AC-5", Text = "FDR" },
                new SelectListItem { Value = "AC-1", Text = "SOD" },
            };

            //assigning SelectListItem to view Bag
            ViewBag.AccountTypes = AccountType;


            ViewBag.Branch = new SelectList(await _bankAccountInfoService.GetAllBranchAsync(), "BranchId", "BranchName");
            ViewBag.Bank = new SelectList(await _bankAccountInfoService.GetAllBankAsync(), "BankId", "BankNames");
            ViewBag.BankBranch = new SelectList(await _bankAccountInfoService.GetAllBankBranchAsync(), "BankBranchId", "BankBranchName");
            if (bankAccountInfo == null)
            {
                return NotFound();
            }
            return View(bankAccountInfo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BankAcInfoId,BranchId,BankId,BankBranchId,AccountTypeId,AccountTypeName,AccountNo,AccountName,OpeningDate,DurationMonth,RateOfInterest")] BankAccountInfoModel bankAccountInfo)
        {
            DateTime ct = bankAccountInfo.OpeningDate;
            DateTime future = ct.AddMonths(bankAccountInfo.DurationMonth);
            bankAccountInfo.ExpiryDate = future;
            if (bankAccountInfo.AccountTypeId == "AC-2")
            {
                bankAccountInfo.AccountTypeName = "STD";
            }
            else if (bankAccountInfo.AccountTypeId == "AC-4")
            {
                bankAccountInfo.AccountTypeName = "Monthly Interest";
            }
            else if (bankAccountInfo.AccountTypeId == "AC-5")
            {
                bankAccountInfo.AccountTypeName = "FDR";
            }
            else if (bankAccountInfo.AccountTypeId == "AC-1")
            {
                bankAccountInfo.AccountTypeName = "SOD";
            }
            //bankAccountInfo.ExpiryDate = DateTime.Now;
            bankAccountInfo.AccountStatus = "Active";
            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
            if (ModelState.IsValid)
            {
                try
                {
                    await _bankAccountInfoService.UpdateAsync(bankAccountInfo);
                    TempData["AlertMessage"] = "Bank Information Update Successfully!!!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BankAccountInfoExistsAsync(bankAccountInfo.BankAcInfoId))
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
            return View(bankAccountInfo);
        }

        private async Task<bool> BankAccountInfoExistsAsync(int id)
        {
            return await _bankAccountInfoService.BankAccountInfoExistsAsync(id);
        }
        //[HttpPost]
        public async Task<IActionResult> Delete(DefaultRequest request)
        {
            var bankName = await _bankAccountInfoService.GetByIdAsync(request.Id);
            if (bankName != null)
            {
                await _bankAccountInfoService.DeleteAsync(request.Id);
            }
            return Ok(new DefaultResponse("Success"));
        }
    }

}
