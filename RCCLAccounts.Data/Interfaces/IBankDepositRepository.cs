using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Interfaces
{
    public interface IBankDepositRepository
    {
        Task<List<BankDeposit>> GetAllBankDeposit();
        Task <int> CreateAsync(BankDeposit bankDeposit);
        Task<BankDeposit> GetByIdAsync(int? id);
        Task <int> UpdateAsync(BankDeposit bankDeposit);
        Task DeleteAsync(int id);
        Task<bool> BankAccountExistsAsync(int? id);

        Task<List<BankAccountInfo>> GetBankAccount();
        Task<List<Department>> GetDepartment();
        Task<List<BranchInformation>> GetBranches();

        Task<BankAccountInfo> GetBankBranchName(int AccountId);
    }
}
