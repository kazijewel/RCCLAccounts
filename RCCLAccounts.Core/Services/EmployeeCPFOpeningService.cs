using Mapster;
using MapsterMapper;
using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Data.Entities;
using ProvidentFund.Data.Interfaces;

using ProvidentFund.Data.Repositories;

namespace ProvidentFund.Core.Services
{
    public class EmployeeCPFOpeningService : IEmployeeCPFOpeningService
	{
        private readonly IEmployeeCPFOpeningRepository _employeeCPFOpeningRepository;
        private readonly IMapper _mapper;

        public EmployeeCPFOpeningService(IEmployeeCPFOpeningRepository employeeCPFOpeningRepository, IMapper mapper)
        {
			_employeeCPFOpeningRepository = employeeCPFOpeningRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(EmployeeCPFOpeningModel employeeCPFOpeningModel)
        {
            var employeeCPFOpening = employeeCPFOpeningModel.Adapt<Data.Entities.EmployeeCPFOpening>(); 
            await _employeeCPFOpeningRepository.CreateAsync(employeeCPFOpening);
        }
        public async Task<List<EmployeeCPFOpeningModel>> GetAllEmployeeCPFOpeningAsync()
        {
            var employeeCPFOpening = await _employeeCPFOpeningRepository.GetAllEmployeeCPFOpeningAsync();
            List<EmployeeCPFOpeningModel> loanInfolist = new List<EmployeeCPFOpeningModel>();
            foreach (var empCPFOpening in employeeCPFOpening)
            {

                EmployeeCPFOpeningModel employeeCPFOpeningModel = new EmployeeCPFOpeningModel();
                employeeCPFOpeningModel.Id = empCPFOpening.Id;
                employeeCPFOpeningModel.EmpolyeeName = empCPFOpening.employeeInfo.EmployeeNo + " - " + empCPFOpening.employeeInfo.EmployeeName;
                employeeCPFOpeningModel.OpeningDate = empCPFOpening.OpeningDate;
                employeeCPFOpeningModel.OpOwnDepositeAmt = empCPFOpening.OpOwnDepositeAmt;
                employeeCPFOpeningModel.OpRCCLContributionAmt = empCPFOpening.OpRCCLContributionAmt;
                employeeCPFOpeningModel.OpInterestDistributionAmt = empCPFOpening.OpInterestDistributionAmt;
                employeeCPFOpeningModel.OpRCCLInterestDistributionAmt = empCPFOpening.OpRCCLInterestDistributionAmt;
                loanInfolist.Add(employeeCPFOpeningModel);


            }

            //return employees.Adapt<List<EmployeeInfoVM>>();
            return loanInfolist;
            //return employeeCPFOpening.Adapt<List<EmployeeCPFOpeningModel>>();
        }
        public async Task<EmployeeCPFOpeningModel> GetByIdAsync(int? id)
        {
            var employeeCPFOpening = await _employeeCPFOpeningRepository.GetByIdAsync(id);

            EmployeeCPFOpeningModel employeeCPFOpeningModel = new EmployeeCPFOpeningModel();
            employeeCPFOpeningModel.Id = employeeCPFOpening.Id;
            employeeCPFOpeningModel.EmpolyeeId = employeeCPFOpening.EmpolyeeId;
            employeeCPFOpeningModel.EmpolyeeName = employeeCPFOpening.employeeInfo.EmployeeNo + " - " + employeeCPFOpening.employeeInfo.EmployeeName;;
            employeeCPFOpeningModel.OpeningDate = employeeCPFOpening.OpeningDate;
            employeeCPFOpeningModel.OpOwnDepositeAmt = employeeCPFOpening.OpOwnDepositeAmt;
            employeeCPFOpeningModel.OpRCCLContributionAmt = employeeCPFOpening.OpRCCLContributionAmt;
            employeeCPFOpeningModel.OpInterestDistributionAmt = employeeCPFOpening.OpInterestDistributionAmt;
            employeeCPFOpeningModel.OpRCCLInterestDistributionAmt = employeeCPFOpening.OpRCCLInterestDistributionAmt;

            //var employeeCPFOpeningModel = employeeCPFOpening.Adapt<EmployeeCPFOpeningModel>(); 
            //return employeeCPFOpeningModel;

            return employeeCPFOpeningModel;
        }
        public async Task UpdateAsync(EmployeeCPFOpeningModel employeeCPFOpeningModel)
        {
            var employeeCPFOpening = employeeCPFOpeningModel.Adapt<EmployeeCPFOpening>(); 
            await _employeeCPFOpeningRepository.UpdateAsync(employeeCPFOpening);
        }
        public async Task DeleteAsync(int id)
        {
            await _employeeCPFOpeningRepository.DeleteAsync(id);
        }
        public async Task<bool> EmployeeCPFOpeningExistsAsync(int id)
        {
            return await _employeeCPFOpeningRepository.EmployeeCPFOpeningExistsAsync(id);
        }

        public async Task<List<EmployeeInfo>> GetAllEmpNoAndName()
        {
      
            var employees = await _employeeCPFOpeningRepository.GetAllEmpNoAndName();
            List<EmployeeInfo> employeelist = new List<EmployeeInfo>();

            foreach (var employee in employees)
            {
                EmployeeInfo employeeInfoVM = new EmployeeInfo();
                employeeInfoVM.EmployeeName = employee.EmployeeNo + " - " + employee.EmployeeName;
                employeeInfoVM.EmpolyeeId = employee.EmpolyeeId;            
                employeelist.Add(employeeInfoVM);

            }

            return employeelist;
        }

        
    }
}
