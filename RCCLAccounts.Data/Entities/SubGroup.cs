using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProvidentFund.Data.Entities;

public partial class SubGroup
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int SubId { get; set; }

    [ForeignKey("PrimaryGroup")]
    public int? PrimaryId { get; set; }

    public string PrimaryGroupId { get; set; } = null!;

    public string PrimaryGroupName { get; set; } = null!;

    [ForeignKey("MainGroup")]
    public int? MainId { get; set; }
    public string MainGroupId { get; set; } = null!;

    public string MainGroupName { get; set; } = null!;

    public string SubGroupId { get; set; } = null!;

    public string SubGroupName { get; set; } = null!;

    public string EntryFrom { get; set; } = null!;

    public int Active { get; set; }

    public string CompanyId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserIp { get; set; } = null!;

    public DateTime EntryTime { get; set; }

    public string? SubGroupCode { get; set; }
    public virtual PrimaryGroup PrimaryGroup { get; set; }
    public virtual MainGroup MainGroup { get; set; }

}
