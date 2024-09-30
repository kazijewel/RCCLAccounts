using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Data.Entities;

public partial class UdLedger
{
    [Key]
    public int  AutoId { get; set; }

    public DateTime OpeningDate { get; set; }

    public string FiscalYearId { get; set; } = null!;

    public string PrimaryGroupId { get; set; } = null!;

    public string MainGroupId { get; set; } = null!;

    public string SubGroupId { get; set; } = null!;

    public string LedgerId { get; set; } = null!;

    public string LedgerCode { get; set; } = null!;

    public string LedgerName { get; set; } = null!;

    public string ParentId { get; set; } = null!;

    public string CreateFrom { get; set; } = null!;

    public string LedgerType { get; set; } = null!;

    public int CreditLimit { get; set; }

    public int Active { get; set; }

    public string CompanyId { get; set; } = null!;

    public string EntryFrom { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserIp { get; set; } = null!;

    public string UdFlag { get; set; } = null!;

    public DateTime EntryTime { get; set; }
}
