using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class seed_data_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "CountryName" },
                values: new object[] { 1, "Netherlands" });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "City", "CountryId", "HouseNumber", "PostalCode", "Street" },
                values: new object[] { 1, "Den Bosch", 1, "1", "0000 AA", "01" });

            migrationBuilder.InsertData(
                table: "Standards",
                columns: new[] { "Id", "CountryId", "Description", "Value" },
                values: new object[,]
                {
                    { "Coli", 1, "aantal minuten per Coli uitladen.", 5 },
                    { "Kasiere", 1, "1 Kasiere per uur per aantal klanten.", 30 },
                    { "Medewerker", 1, "1 medePerCustomer per uur per aantal klanten.", 100 },
                    { "Spiegelen", 1, "aantal seconde voor medePerCustomer per meter.", 30 },
                    { "VakkenVullen", 1, "aantal minuten Vakken vullen per Coli.", 30 }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Name", "BranchId", "Description" },
                values: new object[,]
                {
                    { 0, 1, "Vegetables_Fruit" },
                    { 1, 1, "Meat" },
                    { 2, 1, "Fish" },
                    { 3, 1, "Cheese_Milk" },
                    { 4, 1, "Bread" },
                    { 5, 1, "Cosmetics" },
                    { 6, 1, "Checkout" },
                    { 7, 1, "Stockroom" },
                    { 8, 1, "InformationDesk" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Name",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: "Coli");

            migrationBuilder.DeleteData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: "Kasiere");

            migrationBuilder.DeleteData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: "Medewerker");

            migrationBuilder.DeleteData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: "Spiegelen");

            migrationBuilder.DeleteData(
                table: "Standards",
                keyColumn: "Id",
                keyValue: "VakkenVullen");

            migrationBuilder.DeleteData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
