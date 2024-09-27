using Mapster;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;
using ProvidentFund.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Services
{
    public class BankDepositService:IBankDepositService
    {
        public IBankDepositRepository bankdepositRepository = null;
        public BankDepositService(IBankDepositRepository _bankdepositRepository)
        {
            this.bankdepositRepository = _bankdepositRepository;
        }

        public async Task <int>  CreateAsync(BankDepositVM bankDepositVM)
        {
            var bankdeposit = bankDepositVM.Adapt<BankDeposit>(); // Mapster mapper => Converting EmployeeModel to Employee
            
            await bankdepositRepository.CreateAsync(bankdeposit);
            return bankdeposit.Id;
        }

        public async Task DeleteAsync(int id)
        {
            await bankdepositRepository.DeleteAsync(id);
        }

        public async Task<bool> BankAccountExistsAsync(int? id)
        {
          return await  bankdepositRepository.BankAccountExistsAsync(id);
        }

        public async Task<List<BankDepositVM>> GetAllBankDeposit()
        {
            //var bankDeposits = await bankdepositRepository.GetAllBankDeposit();  
            

            //return bankDeposits.Adapt<List<BankDepositVM>>();

            var bankDeposits = await bankdepositRepository.GetAllBankDeposit();
           
            List<BankDepositVM> bankDepositsList = new List<BankDepositVM>();
            foreach (var bankdep in bankDeposits)
            {

                BankDepositVM bankDepositModel= new BankDepositVM();
                bankDepositModel.Id = bankdep.Id;
                bankDepositModel.TransactionDate = bankdep.TransactionDate;
                bankDepositModel.TransactionType = bankdep.TransactionType;
                bankDepositModel.TransactionMode = bankdep.TransactionMode;
                bankDepositModel.Particulars = bankdep.Particulars;

                bankDepositModel.ChequeNo = bankdep.ChequeNo;
                bankDepositModel.DrAmount = bankdep.DrAmount;
                bankDepositModel.CrAmount = bankdep.CrAmount;
                bankDepositModel.AccountName = bankdep.AccountInfo.AccountTypeName + " - " + bankdep.AccountInfo.AccountNo;
                bankDepositsList.Add(bankDepositModel);

            }

            //return employees.Adapt<List<EmployeeInfoVM>>();
            return bankDepositsList;
        }

        public async Task<BankDepositVM> GetByIdAsync(int? id)
        {
            var bankdeposit = await bankdepositRepository.GetByIdAsync(id);
            var bankdepositModel = bankdeposit.Adapt<BankDepositVM>();

            return bankdepositModel;

        }

        public Task<List<BankAccountInfo>> GetBankAccount()
        {
            return bankdepositRepository.GetBankAccount();
        }

      
        public async Task <int> UpdateAsync(BankDepositVM bankDeposit)
        {
            var bankdeposit = bankDeposit.Adapt<BankDeposit>();
            await bankdepositRepository.UpdateAsync(bankdeposit);
            return bankdeposit.Id;

        }

        public Task<BankAccountInfo> GetBankBranchName(int AccountId)
        {
            return bankdepositRepository.GetBankBranchName(AccountId);
        
        }
    }
}
