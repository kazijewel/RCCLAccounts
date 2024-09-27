using System;
using System.Collections.Generic;

namespace ProvidentFund.Data.Entities;

public partial class FiscalYear
{
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
