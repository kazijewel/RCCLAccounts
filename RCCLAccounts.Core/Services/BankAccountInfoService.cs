using Microsoft.EntityFrameworkCore;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;
using ProvidentFund.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using ProvidentFund.Data.Repositories;
using ProvidentFund.Core.Models;
using Mapster;

namespace ProvidentFund.Core.Services
{
    public class BankAccountInfoService : IBankAccountInfoService
    {
     
        private readonly IBankAccountInfoRepository _bankAccountInfoRepository;
        private readonly IMapper _mapper;


        public BankAccountInfoService(IBankAccountInfoRepository bankAccountInfoRepository, IMapper mapper)
        {

            _bankAccountInfoRepository = bankAccountInfoRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(BankAccountInfoModel bankAccountInfoModel)
        {
            var bankAccountInfo = bankAccountInfoModel.Adapt<BankAccountInfo>();
            await _bankAccountInfoRepository.CreateAsync(bankAccountInfo);
        }

        public async Task<List<BankAccountInfoModel>> GetAllBankAccountInfoAsync()
        {
            var bankAccountInfo =
                await _bankAccountInfoRepository.GetAllBankAccountInfoAsync();
            return bankAccountInfo.Adapt<List<BankAccountInfoModel>>();
        }

        public async Task<BankAccountInfoModel> GetByIdAsync(int? id)
        {
            var bankAccountInfo = await _bankAccountInfoRepository.GetByIdAsync(id);
            var bankAccountInfoModel = bankAccountInfo.Adapt<BankAccountInfoModel>(); 
            return bankAccountInfoModel;
        }
        public async Task UpdateAsync(BankAccountInfoModel bankAccountInfoModel)
        {
            var bankAccountInfo = bankAccountInfoModel.Adapt<BankAccountInfo>();
            await _bankAccountInfoRepository.UpdateAsync(bankAccountInfo);
        }
        public async Task DeleteAsync(int id)
        {
            await _bankAccountInfoRepository.DeleteAsync(id);
        }
        public async Task<bool> BankAccountInfoExistsAsync(int id)
        {
            return await _bankAccountInfoRepository.BankAccountInfoExistsAsync(id);
        }

        public async Task<List<BankNameModel>> GetAllBankAsync()
        {
            var bankName = await _bankAccountInfoRepository.GetAllBankAsync();
            return bankName.Adapt<List<BankNameModel>>();
        }

        public async Task<List<BankBranchModel>> GetAllBankBranchAsync()
        {
            var bankBranch = await _bankAccountInfoRepository.GetAllBankBranchAsync();
            return bankBranch.Adapt<List<BankBranchModel>>();
        }

        public async Task<List<BranchInformation>> GetAllBranchAsync()
        {
            var branch = await _bankAccountInfoRepository.GetAllBranchAsync();
            return branch.Adapt<List<BranchInformation>>();
        }
    }
}
