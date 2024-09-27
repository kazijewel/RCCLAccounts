using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProvidentFund.Data.Migrations
{
    /// <inheritdoc />
    public partial class NIDAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NID",
                table: "EmployeeInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NID",
                table: "EmployeeInfos");
        }
    }
}
