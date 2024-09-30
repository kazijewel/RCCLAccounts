using RCCLAccounts.Data.Entities;

namespace RCCLAccounts.Data.Interfaces
{
    public interface IEmployeeCpfledgerRepository
    {
        Task<List<EmployeeCpfledger>> GetAllEmployeeCpfledgerAsync();
        Task CreateAsync(EmployeeCpfledger employeeCpfledger);
        Task<List<EmployeeCpfledger>> GetByIdAsync(string id);
        Task UpdateAsync(EmployeeCpfledger employeeCpfledger);
        Task<bool> EmployeeCpfledgerExistsAsync(int id);
        Task DeleteAsync(string id);
        Task<int> CreateRangeAsync(List<EmployeeCpfledger> employeeCpfledgers);
        Task<List<EmployeeCpfledger>> GetAllCPFDepositeByTransIdAsync(string id);
    }
}
