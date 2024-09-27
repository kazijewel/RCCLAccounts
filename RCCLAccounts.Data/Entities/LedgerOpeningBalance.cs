using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProvidentFund.Data.Entities;

public partial class LedgerOpeningBalance
{
    [Key]
    public int LedgerOpId { get; set; }

    public DateTime? OpeningDate { get; set; }

    public string? FiscalYearId { get; set; }

    [ForeignKey("Ledger")]
    public int PKLegerId { get; set; }

    public string? LedgerId { get; set; }

    public string? LedgerCode { get; set; }

    public string? LedgerName { get; set; }

    public decimal? DrAmount { get; set; }

    public decimal? CrAmount { get; set; }

    public string? CompanyId { get; set; }

    public string? Flag { get; set; }

    public string? UserName { get; set; }

    public string? UserIp { get; set; }

    public DateTime? EntryTime { get; set; }

    public virtual Ledger Ledger { get; set; }
}
