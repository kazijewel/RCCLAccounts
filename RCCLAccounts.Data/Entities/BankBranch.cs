using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Entities
{
    public class  BankBranch
    {
        [Key]
        public int    BankBranchId { get; set; }
        public string BankBranchName { get; set; }
        public string? BranchAddress { get; set; }
        public string? BranchIncharge { get; set; }
        public string? Designation { get; set; }    
        public string? MobileNo { get; set; }
        public string? TelephoneNo { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }

    }
}
