using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Models
{
    public class InterestPostingModel
    {
      
        public int? InterestAutoId{ get; set; }
        public  string? TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
             
        public int LoanInfoId { get; set; }
        public string LoanTypeName { get; set; }
        public string LoanNo { get; set; }
        public string? EmpolyeeName { get; set; }
        public string? BranchName { get; set; }
        public decimal Balance { get; set; }
        public decimal Rate { get; set; }
        public int InterestDay { get; set; }
        public decimal MonthlyProfit { get; set; }
        public decimal ProvisonalProfit { get; set; }
        public decimal TotalProfit { get; set; }
        public string InterestStatus { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? InterestApplyDate { get; set; }
        public string? ApplyUserName { get; set; }
        public virtual LoanInformation loanInfo { get; set; }
    }
}
