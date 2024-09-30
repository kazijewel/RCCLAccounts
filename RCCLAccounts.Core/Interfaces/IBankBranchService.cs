using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;

namespace RCCLAccounts.Core.Interfaces
{
    public interface IBankBranchService
    {
        Task<List<BankBranchModel>> GetAllBankBranchAsync();
        Task CreateAsync(BankBranchModel bankBranchModel);
        Task<BankBranchModel> GetByIdAsync(int? id);
        Task UpdateAsync(BankBranchModel bankBranchModel);
        Task DeleteAsync(int id);
        Task<bool> BankBranchExistsAsync(int id);

    }
}
