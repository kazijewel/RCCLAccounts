using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Data.Entities;

public partial class PrimaryGroup
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int PrimaryId { get; set; }

    public string PrimaryGroupId { get; set; } = null!;

    public string PrimaryGroupName { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public int NoteNo { get; set; }

    public int Active { get; set; }

    public string ItemOf { get; set; } = null!;

    public string CompanyId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserIp { get; set; } = null!;

    public DateTime? EntryTime { get; set; }

    public string? PrimaryGroupCode { get; set; }
}
