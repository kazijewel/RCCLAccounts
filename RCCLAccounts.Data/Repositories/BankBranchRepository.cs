using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RCCLAccounts.Data.Repositories
{
    public class BankBranchRepository : IBankBranchRepository
    {
        private readonly AppDbContext _dbContext;

        public BankBranchRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(BankBranch bankBranch)
        {
            await _dbContext.Set<BankBranch>().AddAsync(bankBranch);
            await _dbContext.SaveChangesAsync();
        }
        async Task<List<BankBranch>> IBankBranchRepository.GetAllBankBranchAsync()
        {
            return await _dbContext.BankBranch.ToListAsync();
        }
        public async Task<BankBranch> GetByIdAsync(int? id)
        {
            return await _dbContext.BankBranch.FindAsync(id);
        }
        public async Task UpdateAsync(BankBranch bankBranch)
        {
            _dbContext.Entry(bankBranch).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var bankBranch = GetByIdAsync(id);
            if (bankBranch != null)
            {
                _dbContext.BankBranch.Remove(bankBranch.Result);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> BankBranchExistsAsync(int id)
        {
            return await _dbContext.BankBranch.AnyAsync(e => e.BankBranchId == id);
        }
    }
}
