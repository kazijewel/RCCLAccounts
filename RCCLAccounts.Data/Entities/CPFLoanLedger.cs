using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Entities
{
    public class CPFLoanLedger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int CPFLedgerId { get; set; }
        public string TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        
       
        [ForeignKey("loanInfo")]
        public int LoanInfoId { get; set; }
        public string LoanTypeName { get; set; }
        public string TransactionType { get; set; }
        public string TransactionMode { get; set; }      
        public string? ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string? Narration { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }

        public DateTime? ValueDate { get; set; }
        public string? InterestType { get; set; }
        public decimal? InterestAmount { get; set; }
        public string? FiscalYearId { get; set; }
        public string? VoucherNo { get; set; }
        public string? EntryForm { get; set; }
        public string? Intflag { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }
        public virtual LoanInformation loanInfo { get; set; }
    }
}
