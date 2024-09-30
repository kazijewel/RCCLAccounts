using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Models
{
    public class BankNameModel
    {
        public int BankId { get; set; }
        [Required]
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
