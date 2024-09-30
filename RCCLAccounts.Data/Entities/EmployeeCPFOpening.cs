using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Data.Entities
{
	public class EmployeeCPFOpening
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("employeeInfo")]
		public int EmpolyeeId { get; set; }
		public DateTime OpeningDate { get; set; }
		public decimal? OpOwnDepositeAmt { get; set; }
		public decimal? OpRCCLContributionAmt { get; set; }
		public decimal? OpInterestDistributionAmt { get; set; }
        public decimal? OpRCCLInterestDistributionAmt { get; set; }
        public string? UserId { get; set; }
		public string? UserName { get; set; }
		public string? UserIp { get; set; }
		public DateTime? EntryTime { get; set; }

		public virtual EmployeeInfo employeeInfo { get; set; }
	}
}
