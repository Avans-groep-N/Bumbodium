using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class standardssubjecttoENG : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 2,
                column: "Subject",
                value: "StockingShelves");

            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 3,
                column: "Subject",
                value: "Cashier");

            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 4,
                column: "Subject",
                value: "Employee");

            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 5,
                column: "Subject",
                value: "Mirror");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 2,
                column: "Subject",
                value: "VakkenVullen");

            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 3,
                column: "Subject",
                value: "Kasiere");

            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 4,
                column: "Subject",
                value: "Medewerker");

            migrationBuilder.UpdateData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: 5,
                column: "Subject",
                value: "Spiegelen");
        }
    }
}
