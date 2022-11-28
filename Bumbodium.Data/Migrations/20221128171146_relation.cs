using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Country_CountryIdCountryName",
                table: "Branch");

            migrationBuilder.RenameColumn(
                name: "CountryIdCountryName",
                table: "Branch",
                newName: "CountryName");

            migrationBuilder.RenameIndex(
                name: "IX_Branch_CountryIdCountryName",
                table: "Branch",
                newName: "IX_Branch_CountryName");

            migrationBuilder.InsertData(
                table: "Country",
                column: "CountryName",
                value: "Netherlands");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Country_CountryName",
                table: "Branch",
                column: "CountryName",
                principalTable: "Country",
                principalColumn: "CountryName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Country_CountryName",
                table: "Branch");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryName",
                keyValue: "Netherlands");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "Branch",
                newName: "CountryIdCountryName");

            migrationBuilder.RenameIndex(
                name: "IX_Branch_CountryName",
                table: "Branch",
                newName: "IX_Branch_CountryIdCountryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Country_CountryIdCountryName",
                table: "Branch",
                column: "CountryIdCountryName",
                principalTable: "Country",
                principalColumn: "CountryName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
