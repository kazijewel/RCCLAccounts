using RCCLAccounts.Core.Models;

namespace RCCLAccounts.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeModel>> GetAllEmployeesAsync();
        Task CreateAsync(EmployeeModel employeeModel);
        Task<EmployeeModel> GetByIdAsync(int? id);
        Task UpdateAsync(EmployeeModel employeeModel);
        Task DeleteAsync(int id);
        Task<bool> EmployeeExistsAsync(int id);
    }
}
