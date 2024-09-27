using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;

namespace ProvidentFund.Core.Interfaces
{
    public interface IEmployeeCPFOpeningService
	{
        Task<List<EmployeeCPFOpeningModel>> GetAllEmployeeCPFOpeningAsync();
        Task CreateAsync(EmployeeCPFOpeningModel employeeCPFOpeningModel);
        Task<EmployeeCPFOpeningModel> GetByIdAsync(int? id);
        Task UpdateAsync(EmployeeCPFOpeningModel employeeCPFOpeningModel);
        Task DeleteAsync(int id);
        Task<bool> EmployeeCPFOpeningExistsAsync(int id);
        Task<List<EmployeeInfo>> GetAllEmpNoAndName();
    }
}
