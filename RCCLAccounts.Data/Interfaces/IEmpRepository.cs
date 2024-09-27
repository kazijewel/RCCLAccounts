using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Interfaces
{
    public interface IEmpRepository
    {
         Task<List<EmployeeInfo>> GetAllEmployeeAsync();
        Task CreateAsync(EmployeeInfo employee);
        Task<EmployeeInfo> GetByIdAsync(int? id);
        Task UpdateAsync(EmployeeInfo  employee);
        Task DeleteAsync(int id);
        Task<bool> EmployeeExistsAsync(int id);

        Task<List<Designation>> GetDesignation();
        Task<List<Department>> GetDepartment();
        Task<List<BranchInformation>> GetBranches();
    }
}
