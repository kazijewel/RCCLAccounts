using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProvidentFund.Data.Migrations
{
    /// <inheritdoc />
    public partial class columnAddEmpTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LeaveDate",
                table: "EmployeeTransferHistory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PostingDate",
                table: "EmployeeTransferHistory",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveDate",
                table: "EmployeeTransferHistory");

            migrationBuilder.DropColumn(
                name: "PostingDate",
                table: "EmployeeTransferHistory");
        }
    }
}
