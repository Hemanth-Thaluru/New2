using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerMicroService.Migrations
{
    public partial class new4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Pending",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pending",
                table: "Loans");
        }
    }
}
