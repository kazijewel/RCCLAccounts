using ProvidentFund.Data.Entities;

namespace ProvidentFund.Data.Interfaces
{
    public interface IBankNameRepository
    {
        Task<List<BankName>> GetAllBankNameAsync();
        Task CreateAsync(BankName bankName);
        Task<BankName> GetByIdAsync(int? id);
        Task UpdateAsync(BankName bankName);
        Task<bool> BankExistsAsync(int id);
        Task DeleteAsync(int id);
    }
}
