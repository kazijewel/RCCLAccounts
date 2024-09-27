using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;

namespace ProvidentFund.Core.Interfaces
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
