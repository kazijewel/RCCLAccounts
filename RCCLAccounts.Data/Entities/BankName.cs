using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Data.Entities
{
    public class BankName
    {
          [Key]
          //From imran
          //From Jewel
          public int BankId { get; set; }
          public string BankNames { get; set; }
          public string? Address { get; set; }
          public string? TelephoneNo { get; set; }
          public string? ManagingDirector { get; set; }
          public string? MobileNo { get; set; }
          public string? UserName { get; set; }
          public string? UserIp { get; set; }
          public DateTime? EntryTime { get; set; }

  
    }
}
