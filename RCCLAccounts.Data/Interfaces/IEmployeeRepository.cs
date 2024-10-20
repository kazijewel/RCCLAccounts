﻿using RCCLAccounts.Data.Entities;

namespace RCCLAccounts.Data.Interfaces
{
    public interface IEmployeeRepository
    {
         Task<List<Employee>> GetAllEmployeeAsync();
        Task CreateAsync(Employee employee);
        Task<Employee> GetByIdAsync(int? id);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
        Task<bool> EmployeeExistsAsync(int id);
    }
}
