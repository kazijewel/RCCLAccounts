using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Models
{
    public class BankAccountInfoModel
    {
        public int BankAcInfoId { get; set; }
        [Display(Name = "Branch Name")]
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int BankId { get; set; }
        public string? BankNames { get; set; }
        public int BankBranchId { get; set; }
        public string? BankBranchName { get; set; }
        public string AccountTypeId { get; set; }
        public string? AccountTypeName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public DateTime OpeningDate { get; set; }
        public int DurationMonth { get; set; }
        public decimal RateOfInterest { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? AccountStatus { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }

        public  BankName? bankName { get; set; }
        public  BankBranch? bankBranch { get; set; }
        public  BranchInformation? branchInformation { get; set; }



    }
}
