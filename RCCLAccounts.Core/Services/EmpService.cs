using Mapster;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using RCCLAccounts.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Services
{
    public class EmpService : IEmpService
    {
        public IEmpRepository empRepository = null;
         public EmpService(IEmpRepository _empRepository) 
        {
            this.empRepository = _empRepository;
        }

        public async Task CreateAsync(EmployeeInfoVM employeeModel)
        {
            var employee = employeeModel.Adapt<EmployeeInfo>(); // Mapster mapper => Converting EmployeeModel to Employee
            await empRepository.CreateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
           await empRepository.DeleteAsync(id);
        }

        public Task<bool> EmployeeExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeInfoVM>> GetAllEmployeesAsync()
        {
            var employees =  await empRepository.GetAllEmployeeAsync();
            List<EmployeeInfoVM> employeelist= new List<EmployeeInfoVM>();

            foreach (var employee in employees)
            { 
            
                EmployeeInfoVM employeeInfoVM = new EmployeeInfoVM();
                employeeInfoVM.EmployeeName=employee.EmployeeName;
                employeeInfoVM.EmpolyeeId=employee.EmpolyeeId;
                employeeInfoVM.EmployeeNo = employee.EmployeeNo;
                employeeInfoVM.DepartmentName = employee.department.Name;
                employeeInfoVM.DesignationName = employee.designation.Name;
                employeeInfoVM.JoiningDate = employee.JoiningDate;
                employeeInfoVM.EmployeeStatus = employee.EmployeeStatus;
                employeeInfoVM.BranchName = employee.branch.BranchName;
                employeeInfoVM.CpfStartDate = employee.CpfStartDate;
                employeeInfoVM.BasicSalary = employee.BasicSalary;
                employeeInfoVM.CompanyContPer = employee.CompanyContPer;
                employeeInfoVM.OwnContPer = employee.OwnContPer;
                employeelist.Add(employeeInfoVM);


            }

            //return employees.Adapt<List<EmployeeInfoVM>>();
            return employeelist;
        }

        public async Task<EmployeeInfoVM> GetByIdAsync(int? id)
        {
            var employeeinfo = await empRepository.GetByIdAsync(id);
            var employeeModel = employeeinfo.Adapt<EmployeeInfoVM>();

            return employeeModel;

        }

        public Task<List<Department>> GetDepartments()
        {
            return empRepository.GetDepartment();
        }

        public Task<List<Designation>> GetDesignations()
        {
            return empRepository.GetDesignation();
        }

        public Task<List<BranchInformation>> GetBranches()
        {
            return empRepository.GetBranches();
        }

        public async Task  UpdateAsync(EmployeeInfoVM employeeModel)
        {
           var employee= employeeModel.Adapt<EmployeeInfo>();
            await empRepository.UpdateAsync(employee);

        }
    }
}
