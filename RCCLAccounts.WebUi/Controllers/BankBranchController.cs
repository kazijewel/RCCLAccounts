using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Core.Services;
using ProvidentFund.Data.Entities;
using ProvidentFund.WebUi.Models;

namespace ProvidentFund.WebUi.Controllers
{
    public class BankBranchController: Controller
    {

        private readonly IBankBranchService _bankBranchService;
        public BankBranchController(IBankBranchService bankBranchService)
        {
            _bankBranchService = bankBranchService;
        }

        public async Task<IActionResult> Index()
        {
            var bankBranch = await _bankBranchService.GetAllBankBranchAsync();
            return View(bankBranch);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
   
        public async Task<IActionResult> Create([Bind("BankBranchId,BankBranchName,BranchAddress,BranchIncharge,Designation,MobileNo,TelephoneNo")] BankBranchModel bankBranch)
        {
            if (ModelState.IsValid)
            {
                await _bankBranchService.CreateAsync(bankBranch);
                return RedirectToAction(nameof(Index));
            }
            return View(bankBranch);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankBranch = await _bankBranchService.GetByIdAsync(id);
            if (bankBranch == null)
            {
                return NotFound();
            }

            return View(bankBranch);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankBranch = await _bankBranchService.GetByIdAsync(id);
            if (bankBranch == null)
            {
                return NotFound();
            }
            return View(bankBranch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BankBranchId,BankBranchName,BranchAddress,BranchIncharge,Designation,MobileNo,TelephoneNo")] BankBranchModel bankBranch)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _bankBranchService.UpdateAsync(bankBranch);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BankBranchExists(bankBranch.BankBranchId))
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
            return View(bankBranch);
        }

        private async Task<bool> BankBranchExists(int id)
        {
            return await _bankBranchService.BankBranchExistsAsync(id);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DefaultRequest request)
        {
            var bankName = await _bankBranchService.GetByIdAsync(request.Id);
            if (bankName != null)
            {
                await _bankBranchService.DeleteAsync(request.Id);
            }
            return Ok(new DefaultResponse("Success"));
        }
    }

}
