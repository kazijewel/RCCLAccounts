using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;

namespace RCCLAccounts.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(Employee employee)
        {
            await _dbContext.Set<Employee>().AddAsync(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Employee> GetByIdAsync(int? id)
        {
            return await _dbContext.Employees.FindAsync(id);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = GetByIdAsync(id);
            if (employee != null)
            {
                _dbContext.Employees.Remove(employee.Result);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async Task<bool> EmployeeExistsAsync(int id)
        {

            return await _dbContext.Employees.AnyAsync(e => e.Id == id);
        }
    }
}
