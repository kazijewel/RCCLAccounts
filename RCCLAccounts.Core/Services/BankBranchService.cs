
using Mapster;
using MapsterMapper;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using RCCLAccounts.Data.Repositories;

namespace RCCLAccounts.Core.Services
{
    public class BankBranchService : IBankBranchService
    {
        private readonly IBankBranchRepository _bankBranchRepository;
        private readonly IMapper _mapper;

        public BankBranchService(IBankBranchRepository bankBranchRepository, IMapper mapper)
        {
            _bankBranchRepository = bankBranchRepository;
            _mapper = mapper;
        }
        public async Task<bool> BankBranchExistsAsync(int id)
        {
            return await _bankBranchRepository.BankBranchExistsAsync(id);
        }

        public async Task CreateAsync(BankBranchModel bankBranchModel)
        {
            var bankBranch = bankBranchModel.Adapt<BankBranch>(); 
            await _bankBranchRepository.CreateAsync(bankBranch);
        }

        public async Task DeleteAsync(int id)
        {
            await _bankBranchRepository.DeleteAsync(id);
        }

        public async Task<List<BankBranchModel>> GetAllBankBranchAsync()
        {
            var bankBranch = await _bankBranchRepository.GetAllBankBranchAsync();
            return bankBranch.Adapt<List<BankBranchModel>>();
        }

        public async Task<BankBranchModel> GetByIdAsync(int? id)
        {
            var bankBranch = await _bankBranchRepository.GetByIdAsync(id);
            var bankBranchModel = bankBranch.Adapt<BankBranchModel>(); 
            return bankBranchModel;
        }

        public async Task UpdateAsync(BankBranchModel bankBranchModel)
        {
            var bankBranch = bankBranchModel.Adapt<BankBranch>();
            await _bankBranchRepository.UpdateAsync(bankBranch);
        }
    }
}
