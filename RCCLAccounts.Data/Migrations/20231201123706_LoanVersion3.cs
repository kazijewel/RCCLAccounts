using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProvidentFund.Data.Migrations
{
    /// <inheritdoc />
    public partial class LoanVersion3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplyUserName",
                table: "LoanInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InterestApplyDate",
                table: "LoanInformation",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplyUserName",
                table: "LoanInformation");

            migrationBuilder.DropColumn(
                name: "InterestApplyDate",
                table: "LoanInformation");
        }
    }
}
