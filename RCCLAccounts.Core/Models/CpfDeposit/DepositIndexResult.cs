namespace RCCLAccounts.Core.Models.CpfDeposit
{
    public class DepositIndexResult
    {
        public int Id { get; set; }
        public required string TransactionId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalCrAmount { get; set; }
        public string?  Narration { get; set; }
    }
}
