using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RCCLAccounts.Data.Entities;
using System.Collections.Generic;

namespace RCCLAccounts.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
 
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerOpeningBalance> LedgerOpeningBalances { get; set; }
        public DbSet<PrimaryGroup> PrimaryGroups { get; set; }
        public DbSet<MainGroup> MainGroups { get; set; } 
        public DbSet<SubGroup> SubGroups { get; set; }
        public DbSet<UdLedger> UdLedgers { get; set; }
        public DbSet<UdVoucher> UdVouchers { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Narration> Narrations { get; set; }
       

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Configure entity mappings, relationships, etc.
        //    // For example:
        //    //modelBuilder.Entity<Employee>().ToTable("Employees");
        //}
    }
}