using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProvidentFund.Data.Migrations
{
    /// <inheritdoc />
    public partial class InterestPosting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_InterestPosting_LoanInfoId",
                table: "InterestPosting",
                column: "LoanInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestPosting");
        }
    }
}
