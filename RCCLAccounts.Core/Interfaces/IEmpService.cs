using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Interfaces
{
    public interface IEmpService
    {
        Task<List<EmployeeInfoVM>> GetAllEmployeesAsync();
        Task CreateAsync(EmployeeInfoVM employeeModel);
        Task<EmployeeInfoVM> GetByIdAsync(int? id);
        Task UpdateAsync(EmployeeInfoVM employeeModel);
        Task DeleteAsync(int id);
        Task<bool> EmployeeExistsAsync(int id);
        Task<List<Department>> GetDepartments();

        Task<List<Designation>> GetDesignations();
        Task<List<BranchInformation>> GetBranches();

    }
}
