using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Entities
{
    public class BankDeposit
    {
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; }

    [ForeignKey("AccountInfo")]
    public int AccountId { get; set; }
    public string TransactionType { get; set; }
    public string TransactionMode { get; set; }
    public string ChequeNo { get; set; }
     
    public string Particulars { get; set;}
     
    public DateTime HonorDate { get; set; }

     public decimal? RateOfInterest { get; set; }
       
     public decimal? VAT { get; set; }

     public decimal Amount { get; set; }

     public string? VoucherNo { get; set; }

     public decimal DrAmount { get; set; }

     public decimal CrAmount { get; set; }

     public virtual BankAccountInfo AccountInfo { get; set; }

        
    }
}
