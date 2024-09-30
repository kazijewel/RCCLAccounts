using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Interfaces
{
    public interface IBankDepositService
    {
        Task<List<BankDepositVM>> GetAllBankDeposit();
        Task <int> CreateAsync(BankDepositVM bankDepositVM);
        Task<BankDepositVM> GetByIdAsync(int? id);
        Task <int> UpdateAsync(BankDepositVM bankDeposit);
        Task DeleteAsync(int id);
        Task<bool> BankAccountExistsAsync(int? id);
        Task<List<BankAccountInfo>> GetBankAccount();       
        public Task<BankAccountInfo> GetBankBranchName(int accountid);
    }
}
