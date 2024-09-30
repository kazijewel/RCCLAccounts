using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Data.Repositories
{
    public class EmpRepository : IEmpRepository
    {
        private readonly AppDbContext _dbContext;

        public EmpRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(EmployeeInfo employee)
        {
            _dbContext.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                _dbContext.EmployeeInfos.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<bool> EmployeeExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EmployeeInfo>> GetAllEmployeeAsync()
        {           
            return await _dbContext.EmployeeInfos.Include(x => x.designation).Include(x=>x.department).Include(x=>x.branch).OrderBy(e => e.JoiningDate).ThenBy(e => e.DateOfBirth).ToListAsync();               
        }

        public async Task<EmployeeInfo> GetByIdAsync(int? id)
        {
            return await _dbContext.EmployeeInfos.FindAsync(id);
        }

        public async Task< List<Department> > GetDepartment()
        {
            return await _dbContext.Departments.ToListAsync();
        }

        public async Task<List<Designation>> GetDesignation()
        {
            
            return await _dbContext.Designations.ToListAsync();
        }

        public async Task UpdateAsync(EmployeeInfo  employee)
        {
            _dbContext.Entry(employee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BranchInformation>> GetBranches()
        {

            return await _dbContext.BranchInformation.ToListAsync();
        }

        
    }
}
