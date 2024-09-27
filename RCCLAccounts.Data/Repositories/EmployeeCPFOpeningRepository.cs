using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;

namespace ProvidentFund.Data.Repositories
{
    public class EmployeeCPFOpeningRepository : IEmployeeCPFOpeningRepository
	{
        private readonly AppDbContext _dbContext;

        public EmployeeCPFOpeningRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(EmployeeCPFOpening employeeCPFOpening)
        {
            await _dbContext.Set<EmployeeCPFOpening>().AddAsync(employeeCPFOpening);
            await _dbContext.SaveChangesAsync();
        }

        async Task<List<EmployeeCPFOpening>> IEmployeeCPFOpeningRepository.GetAllEmployeeCPFOpeningAsync()
        {
            return await _dbContext.EmployeeCPFOpening.Include(x => x.employeeInfo).ToListAsync();
        }
        public async Task<EmployeeCPFOpening> GetByIdAsync(int? id)
        {
           // return await _dbContext.EmployeeCPFOpening. FindAsync(id);

            return await _dbContext.EmployeeCPFOpening.Include(i => i.employeeInfo).FirstOrDefaultAsync(i => i.Id == id.Value);
            
        }
        public async Task UpdateAsync(EmployeeCPFOpening employeeCPFOpening)
        {
            _dbContext.Entry(employeeCPFOpening).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var employeeCPFOpening = GetByIdAsync(id);
            if (employeeCPFOpening != null)
            {
                _dbContext.EmployeeCPFOpening.Remove(employeeCPFOpening.Result);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> EmployeeCPFOpeningExistsAsync(int id)
        {
            return await _dbContext.EmployeeCPFOpening.AnyAsync(e => e.Id == id);
        }

        async Task<List<EmployeeInfo>> IEmployeeCPFOpeningRepository.GetAllEmpNoAndName()
        {
          
            return await _dbContext.EmployeeInfos.ToListAsync();
        }
    }
}
