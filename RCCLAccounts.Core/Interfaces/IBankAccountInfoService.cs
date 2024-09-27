
using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Interfaces
{
    public interface IBankAccountInfoService
    {
        Task<List<BankAccountInfoModel>> GetAllBankAccountInfoAsync();
        Task CreateAsync(BankAccountInfoModel bankAccountInfo);
        Task<BankAccountInfoModel> GetByIdAsync(int? id);
        Task UpdateAsync(BankAccountInfoModel bankAccountInfo);
        Task<bool> BankAccountInfoExistsAsync(int id);
        Task DeleteAsync(int id);
        Task<List<BankNameModel>> GetAllBankAsync();
        Task<List<BankBranchModel>> GetAllBankBranchAsync();
        Task<List<BranchInformation>> GetAllBranchAsync();
    }
}
