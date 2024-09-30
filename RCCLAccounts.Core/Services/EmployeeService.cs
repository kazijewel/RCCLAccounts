using Mapster;
using MapsterMapper;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;

namespace RCCLAccounts.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(EmployeeModel employeeModel)
        {
            var employee = employeeModel.Adapt<Employee>(); // Mapster mapper => Converting EmployeeModel to Employee
            await _employeeRepository.CreateAsync(employee);
        }

        public async Task<EmployeeModel> GetByIdAsync(int? id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            var employeeModel = employee.Adapt<EmployeeModel>(); // Mapster mapper => Converting Employee to EmployeeModel
            return employeeModel;
        }

        public async Task UpdateAsync(EmployeeModel employeeModel)
        {
            var employee = employeeModel.Adapt<Employee>(); // Mapster mapper => Converting and updating EmployeeModel to Employee
            await _employeeRepository.UpdateAsync(employee);
        }

        public async Task DeleteAsync(int id)
        {
            await _employeeRepository.DeleteAsync(id);
        }

        public async Task<List<EmployeeModel>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeeAsync();
            return employees.Adapt<List<EmployeeModel>>();
        }

        public async Task<bool> EmployeeExistsAsync(int id)
        {
            return await _employeeRepository.EmployeeExistsAsync(id);
        }
    }
}
