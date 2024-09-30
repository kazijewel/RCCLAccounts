using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Data.Entities;

public partial class Voucher
{
    [Key]
    public long AutoId { get; set; }

    public string MasterNo { get; set; } = null!;

    public string FiscalYearId { get; set; } = null!;

    public string VoucherNo { get; set; } = null!;

    public DateTime VoucherDate { get; set; }

    public string ChequeNo { get; set; } = null!;

    public string ChequeDate { get; set; } = null!;

    public string VoucherType { get; set; } = null!;

    public string? LedgerId { get; set; }

    public string LedgerCode { get; set; } = null!;

    public string LedgerName { get; set; } = null!;

    public decimal BalanceAmount { get; set; }

    public decimal DrAmount { get; set; }

    public decimal CrAmount { get; set; }

    public string Narration { get; set; } = null!;

    public string TransactionWith { get; set; } = null!;

    public string CostCenterId { get; set; } = null!;

    public string CostCenterName { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int ChequeClear { get; set; }

    public int AuditApprove { get; set; }

    public string AuditBy { get; set; } = null!;

    public DateTime AuditTime { get; set; }

    public string AuditIp { get; set; } = null!;

    public string ApproveBy { get; set; } = null!;

    public DateTime ApproveTime { get; set; }

    public string ApproveIp { get; set; } = null!;

    public string AttachBill { get; set; } = null!;

    public string AttachCheque { get; set; } = null!;

    public string AttachReference { get; set; } = null!;

    public string ReferenceNo { get; set; } = null!;

    public string ReferenceDetails { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string BranchName { get; set; } = null!;

    public string CompanyId { get; set; } = null!;

    public string EntryFrom { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserIp { get; set; } = null!;

    public DateTime EntryTime { get; set; }

    public string? TransactionId { get; set; }
}
