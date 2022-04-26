using Microsoft.EntityFrameworkCore.Migrations;

namespace Baya.DataLayer.Migrations
{
    public partial class mig8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransfereeFamily",
                table: "UserAddresses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransfereeName",
                table: "UserAddresses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransfereeFamily",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "TransfereeName",
                table: "UserAddresses");
        }
    }
}
