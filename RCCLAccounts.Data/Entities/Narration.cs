using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Data.Entities;

public partial class Narration
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int NarrationId { get; set; }
    public string NarrationCode { get; set; } = null!;
    public string NarrationName { get; set; } 
    public string VoucherType { get; set; }
    public int Active { get; set; }
    public string CompanyId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string UserIp { get; set; } = null!;
    public DateTime? EntryTime { get; set; }


}
