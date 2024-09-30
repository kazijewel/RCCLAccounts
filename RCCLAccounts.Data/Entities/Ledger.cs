using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCCLAccounts.Data.Entities;

public partial class Ledger
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int AutoId { get; set; }

    public DateTime OpeningDate { get; set; }

    [ForeignKey("PrimaryGroup")]
    public int? PrimaryId { get; set; }

    public string PrimaryGroupId { get; set; } = null!;

    [ForeignKey("MainGroup")]
    public int? MainId { get; set; }
    public string MainGroupId { get; set; } = null!;

    [ForeignKey("SubGroup")]
    public int? SubId { get; set; }
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

    public DateTime EntryTime { get; set; }

    public virtual PrimaryGroup PrimaryGroup { get; set; }
    public virtual MainGroup MainGroup { get; set; }
    public virtual SubGroup SubGroup { get; set; }
   

}
