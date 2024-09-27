using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RCCLAccounts.Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastLoginDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankBranch",
                columns: table => new
                {
                    BankBranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankBranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchIncharge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranch", x => x.BankBranchId);
                });

            migrationBuilder.CreateTable(
                name: "BankName",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankNames = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagingDirector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankName", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "BranchInformation",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchFax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchInformation", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                columns: table => new
                {
                    FiscalYearId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AutoId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RunningFlag = table.Column<int>(type: "int", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYears", x => x.FiscalYearId);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryGroups",
                columns: table => new
                {
                    PrimaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteNo = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    ItemOf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrimaryGroupCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryGroups", x => x.PrimaryId);
                });

            migrationBuilder.CreateTable(
                name: "UdLedgers",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FiscalYearId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditLimit = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UdFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UdLedgers", x => x.AutoId);
                });

            migrationBuilder.CreateTable(
                name: "UdVouchers",
                columns: table => new
                {
                    AutoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiscalYearId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionWith = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCenterId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCenterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeClear = table.Column<int>(type: "int", nullable: false),
                    AuditApprove = table.Column<int>(type: "int", nullable: false),
                    AuditBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachBill = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachCheque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UdFlag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UdVouchers", x => x.AutoId);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    AutoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FiscalYearId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoucherType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionWith = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCenterId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostCenterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeClear = table.Column<int>(type: "int", nullable: false),
                    AuditApprove = table.Column<int>(type: "int", nullable: false),
                    AuditBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuditTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuditIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApproveIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachBill = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachCheque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.AutoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountInfo",
                columns: table => new
                {
                    BankAcInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    BankBranchId = table.Column<int>(type: "int", nullable: false),
                    AccountTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMonth = table.Column<int>(type: "int", nullable: false),
                    RateOfInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountInfo", x => x.BankAcInfoId);
                    table.ForeignKey(
                        name: "FK_BankAccountInfo_BankBranch_BankBranchId",
                        column: x => x.BankBranchId,
                        principalTable: "BankBranch",
                        principalColumn: "BankBranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountInfo_BankName_BankId",
                        column: x => x.BankId,
                        principalTable: "BankName",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountInfo_BranchInformation_BranchId",
                        column: x => x.BranchId,
                        principalTable: "BranchInformation",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeInfos",
                columns: table => new
                {
                    EmpolyeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nominee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SigniturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NidPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RetiredMentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PresentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OwnContPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompanyContPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    EmployeeStatus = table.Column<int>(type: "int", nullable: false),
                    Cpfstatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CpfStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInfos", x => x.EmpolyeeId);
                    table.ForeignKey(
                        name: "FK_EmployeeInfos_BranchInformation_BranchId",
                        column: x => x.BranchId,
                        principalTable: "BranchInformation",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeInfos_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeInfos_Designations_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentEmployee",
                columns: table => new
                {
                    DepartmentsId = table.Column<int>(type: "int", nullable: false),
                    EmployeesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentEmployee", x => new { x.DepartmentsId, x.EmployeesId });
                    table.ForeignKey(
                        name: "FK_DepartmentEmployee_Departments_DepartmentsId",
                        column: x => x.DepartmentsId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentEmployee_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MainGroups",
                columns: table => new
                {
                    MainId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryId = table.Column<int>(type: "int", nullable: false),
                    PrimaryGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    EntryFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MainGroupCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainGroups", x => x.MainId);
                    table.ForeignKey(
                        name: "FK_MainGroups_PrimaryGroups_PrimaryId",
                        column: x => x.PrimaryId,
                        principalTable: "PrimaryGroups",
                        principalColumn: "PrimaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankDeposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HonorDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateOfInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDeposits_BankAccountInfo_AccountId",
                        column: x => x.AccountId,
                        principalTable: "BankAccountInfo",
                        principalColumn: "BankAcInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCpfledgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpolyeeId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FiscalYearId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    ContributionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCpfledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCpfledgers_EmployeeInfos_EmpolyeeId",
                        column: x => x.EmpolyeeId,
                        principalTable: "EmployeeInfos",
                        principalColumn: "EmpolyeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCPFOpening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpolyeeId = table.Column<int>(type: "int", nullable: false),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpOwnDepositeAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpRCCLContributionAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpInterestDistributionAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OpRCCLInterestDistributionAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCPFOpening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCPFOpening_EmployeeInfos_EmpolyeeId",
                        column: x => x.EmpolyeeId,
                        principalTable: "EmployeeInfos",
                        principalColumn: "EmpolyeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeTransferHistory",
                columns: table => new
                {
                    EmpolyeeTransferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TranserEmpolyeeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nominee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SigniturePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NidPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RetiredMentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OwnContPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CompanyContPer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    EmployeeStatus = table.Column<int>(type: "int", nullable: false),
                    Cpfstatus = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CpfStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    UdFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTransferHistory", x => x.EmpolyeeTransferId);
                    table.ForeignKey(
                        name: "FK_EmployeeTransferHistory_EmployeeInfos_TranserEmpolyeeId",
                        column: x => x.TranserEmpolyeeId,
                        principalTable: "EmployeeInfos",
                        principalColumn: "EmpolyeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanInformation",
                columns: table => new
                {
                    LoanInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoanTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpolyeeId = table.Column<int>(type: "int", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    BankBranchId = table.Column<int>(type: "int", nullable: false),
                    SenctionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenctionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ParticularsOfSecurity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mSecurityValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoanPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RateOfInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoOfInstallment = table.Column<int>(type: "int", nullable: false),
                    AmountPerInstallment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationMonth = table.Column<int>(type: "int", nullable: false),
                    RecommendingOfficerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FieldOfficerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculationMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewOld = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mOpAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastTransDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SusInterestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InterestFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestApplyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplyUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanInformation", x => x.LoanInfoId);
                    table.ForeignKey(
                        name: "FK_LoanInformation_BankBranch_BankBranchId",
                        column: x => x.BankBranchId,
                        principalTable: "BankBranch",
                        principalColumn: "BankBranchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanInformation_BankName_BankId",
                        column: x => x.BankId,
                        principalTable: "BankName",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoanInformation_EmployeeInfos_EmpolyeeId",
                        column: x => x.EmpolyeeId,
                        principalTable: "EmployeeInfos",
                        principalColumn: "EmpolyeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubGroups",
                columns: table => new
                {
                    SubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryId = table.Column<int>(type: "int", nullable: true),
                    PrimaryGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainId = table.Column<int>(type: "int", nullable: true),
                    MainGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubGroupCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubGroups", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_SubGroups_MainGroups_MainId",
                        column: x => x.MainId,
                        principalTable: "MainGroups",
                        principalColumn: "MainId");
                    table.ForeignKey(
                        name: "FK_SubGroups_PrimaryGroups_PrimaryId",
                        column: x => x.PrimaryId,
                        principalTable: "PrimaryGroups",
                        principalColumn: "PrimaryId");
                });

            migrationBuilder.CreateTable(
                name: "CPFLoanLedger",
                columns: table => new
                {
                    CPFLedgerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanInfoId = table.Column<int>(type: "int", nullable: false),
                    LoanTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Narration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterestType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FiscalYearId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Intflag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CPFLoanLedger", x => x.CPFLedgerId);
                    table.ForeignKey(
                        name: "FK_CPFLoanLedger_LoanInformation_LoanInfoId",
                        column: x => x.LoanInfoId,
                        principalTable: "LoanInformation",
                        principalColumn: "LoanInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestPosting",
                columns: table => new
                {
                    InterestAutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanInfoId = table.Column<int>(type: "int", nullable: false),
                    LoanTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoanNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestDay = table.Column<int>(type: "int", nullable: false),
                    MonthlyProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProvisonalProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalProfit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InterestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterestApplyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApplyUserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestPosting", x => x.InterestAutoId);
                    table.ForeignKey(
                        name: "FK_InterestPosting_LoanInformation_LoanInfoId",
                        column: x => x.LoanInfoId,
                        principalTable: "LoanInformation",
                        principalColumn: "LoanInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    AutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrimaryId = table.Column<int>(type: "int", nullable: true),
                    PrimaryGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainId = table.Column<int>(type: "int", nullable: true),
                    MainGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubId = table.Column<int>(type: "int", nullable: true),
                    SubGroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LedgerType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditLimit = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.AutoId);
                    table.ForeignKey(
                        name: "FK_Ledgers_MainGroups_MainId",
                        column: x => x.MainId,
                        principalTable: "MainGroups",
                        principalColumn: "MainId");
                    table.ForeignKey(
                        name: "FK_Ledgers_PrimaryGroups_PrimaryId",
                        column: x => x.PrimaryId,
                        principalTable: "PrimaryGroups",
                        principalColumn: "PrimaryId");
                    table.ForeignKey(
                        name: "FK_Ledgers_SubGroups_SubId",
                        column: x => x.SubId,
                        principalTable: "SubGroups",
                        principalColumn: "SubId");
                });

            migrationBuilder.CreateTable(
                name: "LedgerOpeningBalances",
                columns: table => new
                {
                    LedgerOpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FiscalYearId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PKLegerId = table.Column<int>(type: "int", nullable: false),
                    LedgerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CrAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerOpeningBalances", x => x.LedgerOpId);
                    table.ForeignKey(
                        name: "FK_LedgerOpeningBalances_Ledgers_PKLegerId",
                        column: x => x.PKLegerId,
                        principalTable: "Ledgers",
                        principalColumn: "AutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfo_BankBranchId",
                table: "BankAccountInfo",
                column: "BankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfo_BankId",
                table: "BankAccountInfo",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountInfo_BranchId",
                table: "BankAccountInfo",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BankDeposits_AccountId",
                table: "BankDeposits",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CPFLoanLedger_LoanInfoId",
                table: "CPFLoanLedger",
                column: "LoanInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEmployee_EmployeesId",
                table: "DepartmentEmployee",
                column: "EmployeesId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCpfledgers_EmpolyeeId",
                table: "EmployeeCpfledgers",
                column: "EmpolyeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCPFOpening_EmpolyeeId",
                table: "EmployeeCPFOpening",
                column: "EmpolyeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInfos_BranchId",
                table: "EmployeeInfos",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInfos_DepartmentId",
                table: "EmployeeInfos",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInfos_DesignationId",
                table: "EmployeeInfos",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTransferHistory_TranserEmpolyeeId",
                table: "EmployeeTransferHistory",
                column: "TranserEmpolyeeId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestPosting_LoanInfoId",
                table: "InterestPosting",
                column: "LoanInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerOpeningBalances_PKLegerId",
                table: "LedgerOpeningBalances",
                column: "PKLegerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_MainId",
                table: "Ledgers",
                column: "MainId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_PrimaryId",
                table: "Ledgers",
                column: "PrimaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_SubId",
                table: "Ledgers",
                column: "SubId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanInformation_BankBranchId",
                table: "LoanInformation",
                column: "BankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanInformation_BankId",
                table: "LoanInformation",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanInformation_EmpolyeeId",
                table: "LoanInformation",
                column: "EmpolyeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MainGroups_PrimaryId",
                table: "MainGroups",
                column: "PrimaryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubGroups_MainId",
                table: "SubGroups",
                column: "MainId");

            migrationBuilder.CreateIndex(
                name: "IX_SubGroups_PrimaryId",
                table: "SubGroups",
                column: "PrimaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BankDeposits");

            migrationBuilder.DropTable(
                name: "CPFLoanLedger");

            migrationBuilder.DropTable(
                name: "DepartmentEmployee");

            migrationBuilder.DropTable(
                name: "EmployeeCpfledgers");

            migrationBuilder.DropTable(
                name: "EmployeeCPFOpening");

            migrationBuilder.DropTable(
                name: "EmployeeTransferHistory");

            migrationBuilder.DropTable(
                name: "FiscalYears");

            migrationBuilder.DropTable(
                name: "InterestPosting");

            migrationBuilder.DropTable(
                name: "LedgerOpeningBalances");

            migrationBuilder.DropTable(
                name: "UdLedgers");

            migrationBuilder.DropTable(
                name: "UdVouchers");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BankAccountInfo");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "LoanInformation");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "BankBranch");

            migrationBuilder.DropTable(
                name: "BankName");

            migrationBuilder.DropTable(
                name: "EmployeeInfos");

            migrationBuilder.DropTable(
                name: "SubGroups");

            migrationBuilder.DropTable(
                name: "BranchInformation");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropTable(
                name: "MainGroups");

            migrationBuilder.DropTable(
                name: "PrimaryGroups");
        }
    }
}
