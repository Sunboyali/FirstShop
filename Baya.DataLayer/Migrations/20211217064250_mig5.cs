using Microsoft.EntityFrameworkCore.Migrations;

namespace Baya.DataLayer.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "UserDetails",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Family",
                table: "UserDetails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTel",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ostan",
                table: "UserDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Family",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "HomeTel",
                table: "UserDetails");

            migrationBuilder.DropColumn(
                name: "Ostan",
                table: "UserDetails");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserDetails",
                newName: "FullName");
        }
    }
}
