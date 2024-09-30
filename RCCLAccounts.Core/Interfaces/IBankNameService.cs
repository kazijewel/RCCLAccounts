using RCCLAccounts.Core.Models;


namespace RCCLAccounts.Core.Interfaces
{
    public interface IBankNameService
    {
        Task<List<BankNameModel>> GetAllBankNameAsync();
        Task CreateAsync(BankNameModel bankNameModel);
        Task<BankNameModel> GetByIdAsync(int? id);
        Task UpdateAsync(BankNameModel bankNameModel);
        Task DeleteAsync(int id);
        Task<bool> BankExistsAsync(int id);
    }
}
