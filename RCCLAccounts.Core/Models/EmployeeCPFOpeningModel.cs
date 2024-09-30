using RCCLAccounts.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Models
{
	public class EmployeeCPFOpeningModel
	{

		public int Id { get; set; }
		public int EmpolyeeId { get; set; }
        public string? EmpolyeeName { get; set; }
        public DateTime OpeningDate { get; set; }
		public decimal? OpOwnDepositeAmt { get; set; }
		public decimal? OpRCCLContributionAmt { get; set; }
		public decimal? OpInterestDistributionAmt { get; set; }
        public decimal? OpRCCLInterestDistributionAmt { get; set; }
        public string? UserId { get; set; }
		public string? UserName { get; set; }
		public string? UserIp { get; set; }
		public DateTime? EntryTime { get; set; }

		public virtual EmployeeInfo? EmployeeInfo { get; set; }
	}
}
