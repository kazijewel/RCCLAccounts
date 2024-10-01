using Mapster;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using RCCLAccounts.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Services
{
    public class PrimaryGroupService : IPrimaryGroupService
    {
        public IPrimaryGroupRepository primaryGroupRepository = null;
        public PrimaryGroupService(IPrimaryGroupRepository _primaryGroupRepository)
        {
            this.primaryGroupRepository = _primaryGroupRepository;
        }

        public async Task <int>  CreateAsync(PrimaryGroupModel primaryGroupModel)
        {
            var primaryGroup = primaryGroupModel.Adapt<PrimaryGroup>(); // Mapster mapper => Converting EmployeeModel to Employee
            
            await primaryGroupRepository.CreateAsync(primaryGroup);
            return primaryGroup.PrimaryId;
        }

        public async Task DeleteAsync(int id)
        {
            await primaryGroupRepository.DeleteAsync(id);
        }

        public async Task<bool> PrimaryGroupExistsAsync(int? id)
        {
          return await primaryGroupRepository.PrimaryGroupExistsAsync(id);
        }

        public async Task<List<PrimaryGroupModel>> GetAllPrimaryGroup()
        {
            var primaryGroup = await primaryGroupRepository.GetAllPrimaryGroup();
            return primaryGroup.Adapt<List<PrimaryGroupModel>>();

        }

        public async Task<PrimaryGroupModel> GetByIdAsync(int? id)
        {
            var primaryGroup = await primaryGroupRepository.GetByIdAsync(id);
            var primaryGroupModel = primaryGroup.Adapt<PrimaryGroupModel>();
            return primaryGroupModel;

        }

  
        public async Task <int> UpdateAsync(PrimaryGroupModel primaryGroup)
        {
            var primaryGroups = primaryGroup.Adapt<PrimaryGroup>();
            await primaryGroupRepository.UpdateAsync(primaryGroups);
            return primaryGroup.PrimaryId;

        }

    }
}
