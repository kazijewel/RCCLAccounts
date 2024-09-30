namespace RCCLAccounts.Core.Models
{
    public class BranchInformationModel
    {
        public long AutoId { get; set; }
        public string CompanyId { get; set; } = null!;
        public string CompanyName { get; set; } = null!;
        public string BranchId { get; set; } = null!;
        public string BranchCode { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string BranchTypeId { get; set; } = null!;
        public string BranchTypeName { get; set; } = null!;
        public string BranchAddress { get; set; } = null!;
        public string BranchPhone { get; set; } = null!;
        public string BranchFax { get; set; } = null!;
        public string BranchEmail { get; set; } = null!;
        public string ChargeName { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string ChargeMobile { get; set; } = null!;
        public DateTime StartFrom { get; set; }
        public string UserName { get; set; } = null!;
        public string UserIp { get; set; } = null!;
        public DateTime EntryTime { get; set; }
        public int Active { get; set; }
    }
}
