using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Core.Services;
using ProvidentFund.WebUi.Models;

namespace ProvidentFund.WebUi.Controllers
{
    public class BankNameController: Controller
    {

        private readonly IBankNameService _bankNameService;
        public BankNameController(IBankNameService bankNameService)
        {
            _bankNameService = bankNameService;
        }

        public async Task<IActionResult> Index()
        {
            var bankName = await _bankNameService.GetAllBankNameAsync();
            return View(bankName);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
   
        public async Task<IActionResult> Create([Bind("BankId,BankNames,Address,TelephoneNo,ManagingDirector,MobileNo")] BankNameModel bankName)
        {
            if (ModelState.IsValid)
            {
                await _bankNameService.CreateAsync(bankName);
                return RedirectToAction(nameof(Index));
            }
            return View(bankName);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankName = await _bankNameService.GetByIdAsync(id);
            if (bankName == null)
            {
                return NotFound();
            }

            return View(bankName);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankName = await _bankNameService.GetByIdAsync(id);
            if (bankName == null)
            {
                return NotFound();
            }
            return View(bankName);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("BankId,BankNames,Address,TelephoneNo,ManagingDirector,MobileNo")] BankNameModel bankName)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _bankNameService.UpdateAsync(bankName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BankExists(bankName.BankId))
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
            return View(bankName);
        }

        private async Task<bool> BankExists(int id)
        {
            return await _bankNameService.BankExistsAsync(id);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DefaultRequest request)
        {
            var bankName = await _bankNameService.GetByIdAsync(request.Id);
            if (bankName != null)
            {
                await _bankNameService.DeleteAsync(request.Id);
            }
            return Ok(new DefaultResponse("Success"));
        }
    }

}
