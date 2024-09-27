using ProvidentFund.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Core.Models
{
    public class LoanInformationModel
    {
        public int LoanInfoId { get; set; }
        public string LoanTypeName { get; set; }
        public string LoanNo { get; set; }
        public int EmpolyeeId { get; set; }
        public string? EmpolyeeName { get; set; }
        public int BankId { get; set; }
        public string? BankNames { get; set; }
        public int BankBranchId { get; set; }
        public string? BankBranchName { get; set; }
        public DateTime SenctionDate { get; set; }
        public decimal SenctionAmount { get; set; }
        public string ParticularsOfSecurity { get; set; }
        public decimal mSecurityValue { get; set; }
        public string LoanPurpose { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal RateOfInterest { get; set; }
        public int NoOfInstallment { get; set; }
        public decimal AmountPerInstallment { get; set; }
        public int DurationMonth { get; set; }
        public string RecommendingOfficerName { get; set; }
        public string FieldOfficerName { get; set; }
        public string CalculationMethod { get; set; }
        public string AccountStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public string NewOld { get; set; }
        public decimal mOpAmount { get; set; }
        public DateTime? LastTransDate { get; set; }
        public string? UserName { get; set; }
        public string? UserIp { get; set; }
        public DateTime? EntryTime { get; set; }
        public decimal? SusInterestAmount { get; set; }
        public string? InterestFlag { get; set; }
        public virtual EmployeeInfo? employeeInfo { get; set; }
        public virtual BankName? bankName { get; set; }
        public virtual BankBranch? bankBranch { get; set; }
    }
}
