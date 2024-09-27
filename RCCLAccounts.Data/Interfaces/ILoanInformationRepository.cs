using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Interfaces
{
    public interface ILoanInformationRepository
    {
        Task<List<LoanInformation>> GetAllLoanInfoAsync();
        Task CreateAsync(LoanInformation loanInformation);
        Task<LoanInformation> GetByLoanIdAsync(int? id);
        Task UpdateAsync(LoanInformation loanInformation);
        Task<bool> LoanInfoExistsAsync(int id);
        Task DeleteAsync(int id);
        Task<List<BankName>> GetAllBankAsync();
        Task<List<BankBranch>> GetAllBankBranchAsync();

        Task<List<EmployeeInfo>> GetAllEmployeeAsync();
       
    }
}
