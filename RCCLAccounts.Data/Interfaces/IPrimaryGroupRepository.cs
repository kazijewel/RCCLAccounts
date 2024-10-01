using RCCLAccounts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Data.Interfaces
{
    public interface IPrimaryGroupRepository
    {
        Task<List<PrimaryGroup>> GetAllPrimaryGroup();
        Task <int> CreateAsync(PrimaryGroup bankDeposit);
        Task<PrimaryGroup> GetByIdAsync(int? id);
        Task <int> UpdateAsync(PrimaryGroup bankDeposit);
        Task DeleteAsync(int id);
        Task<bool> PrimaryGroupExistsAsync(int? id);

  
    }
}
