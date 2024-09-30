using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Data.Entities
{
    public class BankAccountInfo
    {
        [Key]
        public int BankAcInfoId { get; set; }
        [ForeignKey("branchInformation")]
        public int BranchId { get; set; }
        [ForeignKey("bankName")]
        public int BankId { get; set; }
        [ForeignKey("bankBranch")] 
        public int BankBranchId { get; set; }   
        public string AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public DateTime OpeningDate { get; set; }
        public int DurationMonth { get; set; }
        public decimal RateOfInterest { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string AccountStatus { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }

        public virtual BankName bankName { get; set; }
        public virtual BankBranch bankBranch { get; set; }
        public virtual BranchInformation branchInformation { get; set; }

    }
}
