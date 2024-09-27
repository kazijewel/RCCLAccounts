using Mapster;
using MapsterMapper;
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
    public class LoanInformationService : ILoanInformationService
    {

        private readonly ILoanInformationRepository _loanInformationRepository;
        private readonly IMapper _mapper;

        public LoanInformationService(ILoanInformationRepository loanInformationRepository, IMapper mapper)
        {

            _loanInformationRepository = loanInformationRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(LoanInformationModel loanInformationModel)
        {
            var loanInformation = loanInformationModel.Adapt<LoanInformation>();
            await _loanInformationRepository.CreateAsync(loanInformation);
        }

        public async Task<List<LoanInformationModel>> GetAllLoanInfoAsync()
        {
            var loanInformation = await _loanInformationRepository.GetAllLoanInfoAsync();
            List<LoanInformationModel> loanInfolist = new List<LoanInformationModel>();

            foreach (var loan in loanInformation)
            {

                LoanInformationModel loanInformationModel = new LoanInformationModel();
                loanInformationModel.LoanInfoId = loan.LoanInfoId;
                loanInformationModel.LoanNo = loan.LoanNo;
                loanInformationModel.EmpolyeeName = loan.employeeInfo.EmployeeNo+'-'+ loan.employeeInfo.EmployeeName;
                loanInformationModel.BankNames = loan.bankName.BankNames;
                loanInformationModel.LoanTypeName = loan.LoanTypeName;
                loanInformationModel.SenctionDate = loan.SenctionDate;
                loanInformationModel.SenctionAmount = loan.SenctionAmount;
                loanInformationModel.DurationMonth = loan.DurationMonth;
                loanInformationModel.RateOfInterest = loan.RateOfInterest;
                loanInformationModel.AmountPerInstallment = loan.AmountPerInstallment;
                loanInformationModel.SusInterestAmount = loan.SusInterestAmount;
                loanInfolist.Add(loanInformationModel);


            }

            //return employees.Adapt<List<EmployeeInfoVM>>();
            return loanInfolist;
            //return loanInformation.Adapt<List<LoanInformationModel>>();
        }


        public async Task<LoanInformationModel> GetByLoanIdAsync(int? id)
        {
            var loanInformation = await _loanInformationRepository.GetByLoanIdAsync(id);
            var loanInformationModel = loanInformation.Adapt<LoanInformationModel>();
            return loanInformationModel;
        }

        public async Task UpdateAsync(LoanInformationModel loanInformationModel)
        {
            var loanInformation = loanInformationModel.Adapt<LoanInformation>();
            await _loanInformationRepository.UpdateAsync(loanInformation);
        }

        public async Task DeleteAsync(int id)
        {
            await _loanInformationRepository.DeleteAsync(id);
        }

        public async Task<bool> LoanInfoExistsAsync(int id)
        {
            return await _loanInformationRepository.LoanInfoExistsAsync(id);
        }

        public async Task<List<BankNameModel>> GetAllBankAsync()
        {
            var bankName = await _loanInformationRepository.GetAllBankAsync();
            return bankName.Adapt<List<BankNameModel>>();
        }
        public async Task<List<BankBranchModel>> GetAllBankBranchAsync()
        {
            var bankBranch = await _loanInformationRepository.GetAllBankBranchAsync();
            return bankBranch.Adapt<List<BankBranchModel>>();
        }

        public async Task<List<EmployeeInfoVM>> GetAllEmployeeAsync()
        {
            var employee = await _loanInformationRepository.GetAllEmployeeAsync();
            return employee.Adapt<List<EmployeeInfoVM>>();
        }

    }
}
