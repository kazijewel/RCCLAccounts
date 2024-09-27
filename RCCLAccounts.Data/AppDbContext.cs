using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProvidentFund.Data.Entities;
using System.Collections.Generic;

namespace ProvidentFund.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<BankName> BankName { get; set; }

        /* public DbSet<BranchInformation> Branches { get; set; }*/
        public DbSet<EmployeeCpfledger> EmployeeCpfledgers { get; set; }

        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }

        public DbSet<FiscalYear> FiscalYears { get; set; }

        public DbSet<Ledger> Ledgers { get; set; }

        public DbSet<LedgerOpeningBalance> LedgerOpeningBalances { get; set; }
        public DbSet<PrimaryGroup> PrimaryGroups { get; set; }
        public DbSet<MainGroup> MainGroups { get; set; }
   
        public DbSet<SubGroup> SubGroups { get; set; }

        public DbSet<UdLedger> UdLedgers { get; set; }

        public DbSet<UdVoucher> UdVouchers { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<BankBranch> BankBranch { get; set; }

        public DbSet<BankAccountInfo> BankAccountInfo { get; set; }

        public DbSet<BranchInformation> BranchInformation { get; set; }
        public DbSet<BankDeposit> BankDeposits { get; set; }

        public DbSet<LoanInformation> LoanInformation { get; set; }	
        public DbSet<EmployeeCPFOpening> EmployeeCPFOpening { get; set; }

        public DbSet<CPFLoanLedger> CPFLoanLedger { get; set; }

        public DbSet<EmployeeTransferHistory> EmployeeTransferHistory { get; set; }

        public DbSet<InterestPosting> InterestPosting { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Configure entity mappings, relationships, etc.
        //    // For example:
        //    //modelBuilder.Entity<Employee>().ToTable("Employees");
        //}
    }
}