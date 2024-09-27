using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProvidentFund.Data.Entities;

public partial class MainGroup
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int MainId { get; set; }

    [ForeignKey("PrimaryGroup")]
    public int PrimaryId { get; set; }

    public string PrimaryGroupId { get; set; } = null!;

    public string PrimaryGroupName { get; set; } = null!;

    public string MainGroupId { get; set; } = null!;

    public string MainGroupName { get; set; } = null!;

    public int Active { get; set; }

    public string EntryFrom { get; set; } = null!;

    public string CompanyId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserIp { get; set; } = null!;

    public DateTime EntryTime { get; set; }

    public string? MainGroupCode { get; set; }

    public virtual PrimaryGroup PrimaryGroup { get; set; }
}
