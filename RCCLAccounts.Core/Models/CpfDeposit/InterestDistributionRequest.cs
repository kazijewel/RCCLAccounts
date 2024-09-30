namespace RCCLAccounts.Core.Models.CpfDeposit
{
    public class InterestDistributionRequest
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int? Month { get; set; }
        public decimal InterestPercentage { get; set; }
        public decimal InterestAmount { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime? TransactionDate { get; set; }

        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }
    }
}
