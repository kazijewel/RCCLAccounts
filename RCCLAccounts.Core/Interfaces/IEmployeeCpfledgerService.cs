using RCCLAccounts.Core.Models;
using RCCLAccounts.Core.Models.CpfDeposit;

namespace RCCLAccounts.Core.Interfaces
{
	public interface IEmployeeCpfledgerService
	{
		Task<(int,string)> SaveEmployeeCpfledgerAsync(List<DepositRequest> depositRequests);
		Task<(int, string)> SaveCompanyCpfledgerAsync(List<DepositRequest> depositRequests);
        Task<List<DepositIndexResult>> GetAllCompanyCpfledgersAsync();
        Task<List<DepositIndexResult>> GetAllEmployeeCpfledgersAsync();
        Task DeleteAsync(string id);
        Task<List<EmployeeCpfledgerModel>> GetByIdAsync(string id);
        Task<List<EmployeeCpfledgerModel>> GetAllCPFDepositeByTransIdAsync(string id);

        Task<List<InterestDistributionIndexResult>> GetAllInterestDistributionCpfledgersAsync();
        Task<int> SaveInterestDistributionAsync(List<InterestDistributionRequest> interestDistributionRequest);

        Task<List<InterestDistributionIndexResult>> GetAllRCCLInterestDistributionCpfledgersAsync();
        Task<int> SaveRCCLInterestDistributionAsync(List<InterestDistributionRequest> interestDistributionRequest);
    }
}
