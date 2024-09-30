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
    public class LoanInformationRepository : ILoanInformationRepository
    {
        private readonly AppDbContext _dbContext;

        public LoanInformationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(LoanInformation loanInformation)
        {
            await _dbContext.Set<LoanInformation>().AddAsync(loanInformation);
            await _dbContext.SaveChangesAsync();
        }

        async Task<List<LoanInformation>> ILoanInformationRepository.GetAllLoanInfoAsync()
        {
            // return await _dbContext.BankAccountInfo.ToListAsync();
            return await _dbContext.LoanInformation.Include(x => x.employeeInfo).Include(x => x.bankName).Include(x => x.bankBranch).ToListAsync();
        }

        public async Task<LoanInformation> GetByLoanIdAsync(int? id)
        {
             return await _dbContext.LoanInformation.FindAsync(id);

            //return await _dbContext.LoanInformation.Include(x => x.employeeInfo).Include(x => x.bankName).Include(x => x.bankBranch).FirstOrDefaultAsync(x => x.Id = id);
        }

        public async Task UpdateAsync(LoanInformation loanInformation)
        {
            _dbContext.Entry(loanInformation).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loanInformation = GetByLoanIdAsync(id);
            if (loanInformation != null)
            {
                _dbContext.LoanInformation.Remove(loanInformation.Result);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> LoanInfoExistsAsync(int id)
        {
            return await _dbContext.LoanInformation.AnyAsync(e => e.LoanInfoId == id);
        }

        async Task<List<BankName>> ILoanInformationRepository.GetAllBankAsync()
        {
            return await _dbContext.BankName.ToListAsync();
        }

        async Task<List<BankBranch>> ILoanInformationRepository.GetAllBankBranchAsync()
        {
            return await _dbContext.BankBranch.ToListAsync();
        }

        async Task<List<EmployeeInfo>> ILoanInformationRepository.GetAllEmployeeAsync()
        {
            return await _dbContext.EmployeeInfos.ToListAsync();
        }
    }
}
