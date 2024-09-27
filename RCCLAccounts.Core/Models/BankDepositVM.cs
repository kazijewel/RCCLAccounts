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
    public class BankDepositVM
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Transaction Date is Required")]
        [Display(Name = "Transaction Date")]             
       // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [ForeignKey("AccountInfo")]
        [Required(ErrorMessage = "Account No Is Required")]
        [Display(Name ="Account No")]
        public int AccountId { get; set; }

        [Required(ErrorMessage ="Transaction Type Required")]
        [Display(Name ="Transaction Type")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "Transaction Mode Required")]
        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }

      
        [Display(Name = "Cheque No")]
        [Required]
        public string ChequeNo { get; set; }

        [Required]
        [Display(Name = "particulars")]
        public string? Particulars { get; set; }

        [Display(Name = "Honor Date")]
        public DateTime? HonorDate { get; set; }

        [Display(Name = "Rate Of Interest")]
        public decimal? RateOfInterest { get; set; }

        [Display(Name = "Vat")]
        public decimal? VAT { get; set; }

        [Required]
        [Display(Name ="Amount")]
        public decimal Amount { get; set; }

        public string? BankName { get; set; }
        public string? BankBranch { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? TransactionModeName { get; set; }

        public string? VoucherNo { get; set; }

        public decimal DrAmount { get; set; }

        public decimal CrAmount { get; set; }

        public string? AccountName { get; set; }

        public virtual BankAccountInfo? AccountInfo { get; set; }
    }
}
