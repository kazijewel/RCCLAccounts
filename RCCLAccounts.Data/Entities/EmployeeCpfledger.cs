using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCCLAccounts.Data.Entities;

public class EmployeeCpfledger
{
    [ForeignKey("EmployeeInfo")]
    public int EmpolyeeId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int Id { get; set; }

    public string? TransactionId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? FiscalYearId { get; set; }

    public string? TransactionType { get; set; }

    public string? TransactionMode { get; set; }

    public string? Narration { get; set; }

    public decimal? DrAmount { get; set; }

    public decimal? CrAmount { get; set; }

    public string? VoucherNo { get; set; }

    public string? EntryForm { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserIp { get; set; }

    public DateTime? EntryTime { get; set; }

    public virtual EmployeeInfo EmployeeInfo { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public decimal? ContributionPercentage { get; set; }
    public decimal? BasicSalary { get; set; }
}