using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Data.Entities;

public partial class FiscalYear
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
	public long AutoId { get; set; }

    public string CompanyId { get; set; } = null!;

    public string FiscalYearId { get; set; } = null!;

    public DateTime OpeningDate { get; set; }

    public DateTime ClosingDate { get; set; }

    public int RunningFlag { get; set; }

    public bool? IsClosed { get; set; }

    public string? UserName { get; set; }

    public string? UserIp { get; set; }

    public DateTime? EntryTime { get; set; }
}
