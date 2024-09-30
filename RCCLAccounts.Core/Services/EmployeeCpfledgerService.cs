using Mapster;
using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Core.Models.CpfDeposit;
using RCCLAccounts.Data.Entities;
using RCCLAccounts.Data.Interfaces;
using RCCLAccounts.Data.Repositories;

namespace RCCLAccounts.Core.Services
{
    public class EmployeeCpfledgerService : IEmployeeCpfledgerService
    {
        private readonly IEmployeeCpfledgerRepository _employeeCpfledgerRepository;
        public EmployeeCpfledgerService(IEmployeeCpfledgerRepository employeeCpfledgerRepository)
        {
            _employeeCpfledgerRepository = employeeCpfledgerRepository;
        }

        public async Task<List<DepositIndexResult>> GetAllCompanyCpfledgersAsync()
        {
            var employeeCpfledgers = await _employeeCpfledgerRepository.GetAllEmployeeCpfledgerAsync();
            return employeeCpfledgers
                .Where(x => x.TransactionMode == "RCCL Contribution")
                .GroupBy(x => new { x.TransactionId, x.Month, x.Year, x.Narration })
                .Select(x => new DepositIndexResult
                {
                    TransactionId = x.Key.TransactionId,
                    Month = (int)x.Key.Month,
                    Year = (int)x.Key.Year,
                    TotalCrAmount = (decimal)x.Sum(y => y.CrAmount),
                    Narration = x.Key.Narration
                }).ToList();
        }

        public async Task<List<DepositIndexResult>> GetAllEmployeeCpfledgersAsync()
        {
            var employeeCpfledgers = await _employeeCpfledgerRepository.GetAllEmployeeCpfledgerAsync();
            return employeeCpfledgers
                .Where(x => x.TransactionMode == "CPF Deposit")
                .GroupBy(x => new { x.TransactionId, x.Month, x.Year, x.Narration })
                .Select(x => new DepositIndexResult
                {
                    TransactionId = x.Key.TransactionId,
                    Month = (int)x.Key.Month,
                    Year = (int)x.Key.Year,
                    TotalCrAmount = (decimal)x.Sum(y => y.CrAmount),
                    Narration = x.Key.Narration
                }).ToList();
        }

        public async Task<(int, string)> SaveCompanyCpfledgerAsync(List<DepositRequest> depositRequests)
        {
            string Year = depositRequests.First().Year.ToString();
            string monthName = new DateTime(depositRequests.First().Year, depositRequests.First().Month, 1).Date.ToString("MMMM");

            var transactionId = Guid.NewGuid().ToString();

            int result = await _employeeCpfledgerRepository
                .CreateRangeAsync(depositRequests.Select(x => new EmployeeCpfledger
                {
                    EmpolyeeId = x.EmployeeId,
                    Month = x.Month,
                    Year = x.Year,
                    EntryForm = "RCCL Contribution",
                    TransactionType = "Transfer",
                    TransactionMode = "RCCL Contribution",
                    // TransactionDate = new DateTime(x.Year, x.Month, 1),
                    TransactionDate = new DateTime(x.Year, x.Month, DateTime.DaysInMonth(x.Year, x.Month)),
                    EntryTime = DateTime.Now,
                    Narration = "RCCL Contribution for the month of " + monthName + " " + Year,
                    DrAmount = 0,
                    CrAmount = x.BasicSalary * (x.ContributionPercentage / 100),
                    ContributionPercentage = x.ContributionPercentage,
                    BasicSalary = x.BasicSalary,
                    TransactionId = transactionId
                }).ToList());
            return (result, transactionId);
        }

        public async Task<(int, string)> SaveEmployeeCpfledgerAsync(List<DepositRequest> depositRequests)
        {
            string Year = depositRequests.First().Year.ToString();
            string monthName = new DateTime(depositRequests.First().Year, depositRequests.First().Month, 1).Date.ToString("MMMM");
            var transactionId = Guid.NewGuid().ToString();
            int result= await _employeeCpfledgerRepository
                .CreateRangeAsync(depositRequests.Select(x => new EmployeeCpfledger
                {
                    EmpolyeeId = x.EmployeeId,
                    Month = x.Month,
                    Year = x.Year,
                    EntryForm = "CPF Employee Deposit",
                    TransactionType = "Transfer",
                    TransactionMode = "CPF Deposit",
                    // TransactionDate = new DateTime(x.Year, x.Month, 1),
                    TransactionDate = new DateTime(x.Year, x.Month, DateTime.DaysInMonth(x.Year, x.Month)),
                    EntryTime = DateTime.Now,
                    Narration = "Own Contribution for the month of " + monthName + " " + Year,
                    DrAmount = 0,
                    CrAmount = x.BasicSalary * (x.ContributionPercentage / 100),
                    ContributionPercentage = x.ContributionPercentage,
                    BasicSalary = x.BasicSalary,
                    TransactionId = transactionId
                }).ToList());

            return (result, transactionId);
        }

        public async Task<List<EmployeeCpfledgerModel>> GetByIdAsync(string id)
        {
            var cpfledger = await _employeeCpfledgerRepository.GetByIdAsync(id);

            return cpfledger.Adapt<List<EmployeeCpfledgerModel>>();
        }

        public async Task DeleteAsync(string id)
        {
            await _employeeCpfledgerRepository.DeleteAsync(id);
        }

        public async Task<List<EmployeeCpfledgerModel>> GetAllCPFDepositeByTransIdAsync(string id)
        {
            var cpfledger = await _employeeCpfledgerRepository.GetAllCPFDepositeByTransIdAsync(id);
          
            List<EmployeeCpfledgerModel> Cpfledgerlist = new List<EmployeeCpfledgerModel>();

            foreach (var EmpCPFLedger in cpfledger)
            {

                EmployeeCpfledgerModel EmployeeCpfledgerModel = new EmployeeCpfledgerModel();
                EmployeeCpfledgerModel.TransactionId = EmpCPFLedger.TransactionId;
                EmployeeCpfledgerModel.EmpolyeeNo= EmpCPFLedger.EmployeeInfo.EmployeeNo;
                EmployeeCpfledgerModel.EmpolyeeName = EmpCPFLedger.EmployeeInfo.EmployeeName;
                EmployeeCpfledgerModel.BasicSalary = EmpCPFLedger.BasicSalary;
                EmployeeCpfledgerModel.ContributionPercentage = EmpCPFLedger.ContributionPercentage;
                EmployeeCpfledgerModel.DrAmount = EmpCPFLedger.DrAmount;
                EmployeeCpfledgerModel.CrAmount = EmpCPFLedger.CrAmount;
                EmployeeCpfledgerModel.Month = (int)EmpCPFLedger.Month;
                EmployeeCpfledgerModel.Year = (int)EmpCPFLedger.Year;
                EmployeeCpfledgerModel.Narration = EmpCPFLedger.Narration;
                EmployeeCpfledgerModel.TransactionMode = EmpCPFLedger.TransactionMode;
                EmployeeCpfledgerModel.BranchName = EmpCPFLedger.EmployeeInfo.branch.BranchName;

                Cpfledgerlist.Add(EmployeeCpfledgerModel);


            }
            
            return Cpfledgerlist;        
        }

        public async Task<List<InterestDistributionIndexResult>> GetAllInterestDistributionCpfledgersAsync()
        {
            var employeeCpfledgers = await _employeeCpfledgerRepository.GetAllEmployeeCpfledgerAsync();
            return employeeCpfledgers
                .Where(x => x.TransactionMode == "Own Profit")
                .GroupBy(x => new { x.TransactionId, x.Month, x.Year, x.Narration })
                .Select(x => new InterestDistributionIndexResult
                {
                    TransactionId = x.Key.TransactionId,
                    Month = (int)x.Key.Month,
                    Year = (int)x.Key.Year,
                    TotalCrAmount = (decimal)x.Sum(y => y.CrAmount),
                    Narration = x.Key.Narration
                }).ToList();
        }

        public async Task<int> SaveInterestDistributionAsync(List<InterestDistributionRequest> interestDistributionRequest)
        {
            var transactionId = Guid.NewGuid().ToString();


            return await _employeeCpfledgerRepository
                .CreateRangeAsync(interestDistributionRequest.Select(x => new EmployeeCpfledger
                {
                    EmpolyeeId = x.EmployeeId,
                    Month = 0,
                    Year = x.Year,
                    EntryForm = "Own Profit Distribution",
                    TransactionType = "Transfer",
                    TransactionMode = "Own Profit",
                    TransactionDate = x.TransactionDate,
                    EntryTime = DateTime.Now,
                    Narration = $"Own Profit distribution for the Year of {x.Year}",
                    DrAmount = 0,
                    CrAmount = x.InterestAmount,
                    ContributionPercentage = x.InterestPercentage,
                    BasicSalary = x.BasicSalary,
                    TransactionId = transactionId
                  
                }).ToList());
        }



        public async Task<List<InterestDistributionIndexResult>> GetAllRCCLInterestDistributionCpfledgersAsync()
        {
            var employeeCpfledgers = await _employeeCpfledgerRepository.GetAllEmployeeCpfledgerAsync();
            return employeeCpfledgers
                .Where(x => x.TransactionMode == "RCCL Profit")
                .GroupBy(x => new { x.TransactionId, x.Month, x.Year, x.Narration })
                .Select(x => new InterestDistributionIndexResult
                {
                    TransactionId = x.Key.TransactionId,
                    Month = (int)x.Key.Month,
                    Year = (int)x.Key.Year,
                    TotalCrAmount = (decimal)x.Sum(y => y.CrAmount),
                    Narration = x.Key.Narration
                }).ToList();
        }

        public async Task<int> SaveRCCLInterestDistributionAsync(List<InterestDistributionRequest> interestDistributionRequest)
        {
            var transactionId = Guid.NewGuid().ToString();


            return await _employeeCpfledgerRepository
                .CreateRangeAsync(interestDistributionRequest.Select(x => new EmployeeCpfledger
                {
                    EmpolyeeId = x.EmployeeId,
                    Month = 0,
                    Year = x.Year,
                    EntryForm = "RCCL Profit Distribution",
                    TransactionType = "Transfer",
                    TransactionMode = "RCCL Profit",
                    TransactionDate = x.TransactionDate,
                    EntryTime = DateTime.Now,
                    Narration = $"RCCL Profit distribution for the Year of {x.Year}",
                    DrAmount = 0,
                    CrAmount = x.InterestAmount,
                    ContributionPercentage = x.InterestPercentage,
                    BasicSalary = x.BasicSalary,
                    TransactionId = transactionId

                }).ToList());
        }

    }
}
