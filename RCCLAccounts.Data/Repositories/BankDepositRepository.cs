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
    public class BankDepositRepository:IBankDepositRepository
    {
        private readonly AppDbContext _dbContext;

        public BankDepositRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task <int> CreateAsync(BankDeposit bankDeposit)
        {
            _dbContext.Add(bankDeposit);
            await _dbContext.SaveChangesAsync();
            return bankDeposit.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var bankDeposit = await GetByIdAsync(id);
            if (bankDeposit != null)
            {
                _dbContext.BankDeposits.Remove(bankDeposit);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> BankAccountExistsAsync(int? id)
        {
         var bankAccountInfo= await  _dbContext.BankAccountInfo.FindAsync(id);
            if (bankAccountInfo != null)
                return true;
            else return false;
        }

        public async Task<List<BankDeposit>> GetAllBankDeposit()
        {
            return await _dbContext.BankDeposits.Include(x => x.AccountInfo).ToListAsync();
        }

        public async Task<BankDeposit> GetByIdAsync(int? id)
        {
            return await _dbContext.BankDeposits.FindAsync(id);
        }

        public async Task<List<BankAccountInfo>> GetBankAccount()
        {
            return await _dbContext.BankAccountInfo.ToListAsync();
        }

        public async Task<BankAccountInfo> GetBankBranchName(int AccountId)
        {
            return await _dbContext.BankAccountInfo.Where(x => x.BankAcInfoId == AccountId).Include(x => x.bankBranch).Include(x => x.bankName).Include(x => x.branchInformation).FirstOrDefaultAsync();       
        }

       
        public async  Task <int> UpdateAsync(BankDeposit bankDeposit)
        {
            _dbContext.Entry(bankDeposit).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return bankDeposit.Id;
        }

        public Task<List<Department>> GetDepartment()
        {
            throw new NotImplementedException();
        }

        public Task<List<BranchInformation>> GetBranches()
        {
            throw new NotImplementedException();
        }
    }
}
