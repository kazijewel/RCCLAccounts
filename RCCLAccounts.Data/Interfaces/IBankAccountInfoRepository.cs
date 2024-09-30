using RCCLAccounts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Data.Interfaces
{
    public interface IBankAccountInfoRepository
    {
        Task<List<BankAccountInfo>> GetAllBankAccountInfoAsync();
        Task CreateAsync(BankAccountInfo bankAccountInfo);
        Task<BankAccountInfo> GetByIdAsync(int? id);
        Task UpdateAsync(BankAccountInfo bankAccountInfo);
        Task<bool> BankAccountInfoExistsAsync(int id);
        Task DeleteAsync(int id);
        Task<List<BankName>> GetAllBankAsync();
        Task<List<BankBranch>> GetAllBankBranchAsync();
        Task<List<BranchInformation>> GetAllBranchAsync();

    }
}
