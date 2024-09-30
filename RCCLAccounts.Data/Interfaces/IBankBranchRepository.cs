using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;

namespace RCCLAccounts.Data.Interfaces
{
    public interface IBankBranchRepository
    {
         Task<List<BankBranch>> GetAllBankBranchAsync();
         Task CreateAsync(BankBranch bankBranch);
         Task<BankBranch> GetByIdAsync(int? id);
         Task UpdateAsync(BankBranch bankBranch);
         Task<bool> BankBranchExistsAsync(int id);
         Task DeleteAsync(int id);
    }
}
