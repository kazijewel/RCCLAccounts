using Microsoft.EntityFrameworkCore;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Repositories
{
    public class BankAccountInfoRepository : IBankAccountInfoRepository
    {
        private readonly AppDbContext _dbContext;

        public BankAccountInfoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(BankAccountInfo bankAccountInfo)
        {
            await _dbContext.Set<BankAccountInfo>().AddAsync(bankAccountInfo);
            await _dbContext.SaveChangesAsync();
        }

        async Task<List<BankAccountInfo>> IBankAccountInfoRepository.GetAllBankAccountInfoAsync()
        {
           
            return await _dbContext.BankAccountInfo.Include(x => x.branchInformation).Include(x => x.bankName).Include(x => x.bankBranch).ToListAsync();
        }

        public async Task<BankAccountInfo> GetByIdAsync(int? id)
        {
            return await _dbContext.BankAccountInfo.FindAsync(id);
        }

        public async Task UpdateAsync(BankAccountInfo bankAccountInfo)
        {
            _dbContext.Entry(bankAccountInfo).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bankAccountInfo = GetByIdAsync(id);
            if (bankAccountInfo != null)
            {
                _dbContext.BankAccountInfo.Remove(bankAccountInfo.Result);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<bool> BankAccountInfoExistsAsync(int id)
        {
            return await _dbContext.BankAccountInfo.AnyAsync(e => e.BankAcInfoId == id);
        }

        async Task<List<BankName>> IBankAccountInfoRepository.GetAllBankAsync()
        {
            return await _dbContext.BankName.ToListAsync();
        }

        async Task<List<BankBranch>> IBankAccountInfoRepository.GetAllBankBranchAsync()
        {
            return await _dbContext.BankBranch.ToListAsync();
        }

        async Task<List<BranchInformation>> IBankAccountInfoRepository.GetAllBranchAsync()
        {
            return await _dbContext.BranchInformation.ToListAsync();
        }

    }
}
