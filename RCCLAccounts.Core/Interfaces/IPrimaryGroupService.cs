using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Interfaces
{
    public interface IPrimaryGroupService
    {
        Task<List<PrimaryGroupModel>> GetAllPrimaryGroup();
        Task <int> CreateAsync(PrimaryGroupModel primaryGroup);
        Task<PrimaryGroupModel> GetByIdAsync(int? id);
        Task <int> UpdateAsync(PrimaryGroupModel primaryGroup);
        Task DeleteAsync(int id);
        Task<bool> PrimaryGroupExistsAsync(int? id);
       
    }
}
