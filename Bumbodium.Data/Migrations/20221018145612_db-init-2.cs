using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class dbinit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Streed",
                table: "Filiaal",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "HomeNumber",
                table: "Filiaal",
                newName: "HouseNumber");

            migrationBuilder.RenameColumn(
                name: "Function",
                table: "Employee",
                newName: "WorkFunction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Filiaal",
                newName: "Streed");

            migrationBuilder.RenameColumn(
                name: "HouseNumber",
                table: "Filiaal",
                newName: "HomeNumber");

            migrationBuilder.RenameColumn(
                name: "WorkFunction",
                table: "Employee",
                newName: "Function");
        }
    }
}
