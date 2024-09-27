using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Interfaces
{
    public interface ILoanInformationService
    {
        Task<List<LoanInformationModel>> GetAllLoanInfoAsync();
        Task CreateAsync(LoanInformationModel loanInformation);
        Task<LoanInformationModel> GetByLoanIdAsync(int? id);
        Task UpdateAsync(LoanInformationModel loanInformation);
        Task<bool> LoanInfoExistsAsync(int id);
        Task DeleteAsync(int id);
        Task<List<BankNameModel>> GetAllBankAsync();
        Task<List<BankBranchModel>> GetAllBankBranchAsync();
        Task<List<EmployeeInfoVM>> GetAllEmployeeAsync();
    }
}
