using RCCLAccounts.Data.Entities;

namespace RCCLAccounts.Data.Interfaces
{
    public interface IEmployeeCPFOpeningRepository
	{
        Task<List<EmployeeCPFOpening>> GetAllEmployeeCPFOpeningAsync();
        Task CreateAsync(EmployeeCPFOpening employeeCPFOpening);
        Task<EmployeeCPFOpening> GetByIdAsync(int? id);
        Task UpdateAsync(EmployeeCPFOpening employeeCPFOpening);
        Task<bool> EmployeeCPFOpeningExistsAsync(int id);
        Task DeleteAsync(int id);
        Task<List<EmployeeInfo>> GetAllEmpNoAndName();
    }
}
