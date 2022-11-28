using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class update_relationships_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Standards");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Standards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryIdCountryName",
                table: "Branch",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    CountryName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standards_CountryName",
                table: "Standards",
                column: "CountryName");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CountryIdCountryName",
                table: "Branch",
                column: "CountryIdCountryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Country_CountryIdCountryName",
                table: "Branch",
                column: "CountryIdCountryName",
                principalTable: "Country",
                principalColumn: "CountryName",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Country_CountryName",
                table: "Standards",
                column: "CountryName",
                principalTable: "Country",
                principalColumn: "CountryName",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Country_CountryIdCountryName",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Country_CountryName",
                table: "Standards");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Standards_CountryName",
                table: "Standards");

            migrationBuilder.DropIndex(
                name: "IX_Branch_CountryIdCountryName",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Standards");

            migrationBuilder.DropColumn(
                name: "CountryIdCountryName",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Standards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Branch",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}
