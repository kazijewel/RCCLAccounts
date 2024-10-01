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
    public class PrimaryGroupRepository : IPrimaryGroupRepository
    {
        private readonly AppDbContext _dbContext;

        public PrimaryGroupRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task <int> CreateAsync(PrimaryGroup primaryGroup)
        {
            _dbContext.Add(primaryGroup);
            await _dbContext.SaveChangesAsync();
            return primaryGroup.PrimaryId;
        }

        public async Task DeleteAsync(int id)
        {
            var primaryGroup = await GetByIdAsync(id);
            if (primaryGroup != null)
            {
                _dbContext.PrimaryGroups.Remove(primaryGroup);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> PrimaryGroupExistsAsync(int? id)
        {
         var primaryGroup = await  _dbContext.PrimaryGroups.FindAsync(id);
            if (primaryGroup != null)
                return true;
            else return false;
        }

        public async Task<List<PrimaryGroup>> GetAllPrimaryGroup()
        {
            return await _dbContext.PrimaryGroups.ToListAsync();
        }

        public async Task<PrimaryGroup> GetByIdAsync(int? id)
        {
            return await _dbContext.PrimaryGroups.FindAsync(id);
        }

       
        public async  Task <int> UpdateAsync(PrimaryGroup primaryGroup)
        {
            _dbContext.Entry(primaryGroup).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return primaryGroup.PrimaryId;
        }

    
    }
}
