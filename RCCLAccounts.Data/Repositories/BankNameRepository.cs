using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;

namespace RCCLAccounts.Data.Repositories
{
    public class BankNameRepository : IBankNameRepository
    {
        private readonly AppDbContext _dbContext;

        public BankNameRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(BankName bankName)
        {
            await _dbContext.Set<BankName>().AddAsync(bankName);
            await _dbContext.SaveChangesAsync();
        }

        async Task<List<BankName>> IBankNameRepository.GetAllBankNameAsync()
        {
            return await _dbContext.BankName.ToListAsync();
        }
        public async Task<BankName> GetByIdAsync(int? id)
        {
            return await _dbContext.BankName.FindAsync(id);
        }
        public async Task UpdateAsync(BankName bankName)
        {
            _dbContext.Entry(bankName).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var bankName = GetByIdAsync(id);
            if (bankName != null)
            {
                _dbContext.BankName.Remove(bankName.Result);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> BankExistsAsync(int id)
        {
            return await _dbContext.BankName.AnyAsync(e => e.BankId == id);
        }
    }
}
