namespace ProvidentFund.Core.Models.CpfDeposit
{
    public class DepositRequest
    {
        public int EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal ContributionPercentage { get; set; }
        public decimal ContributionAmount { get; set; }
        public decimal BasicSalary { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
