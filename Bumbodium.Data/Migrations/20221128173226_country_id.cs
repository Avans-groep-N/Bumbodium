using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class country_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Country_CountryName",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Country_CountryName",
                table: "Standards");

            migrationBuilder.DropIndex(
                name: "IX_Standards_CountryName",
                table: "Standards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Branch_CountryName",
                table: "Branch");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryName",
                keyValue: "Netherlands");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Standards");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "Branch");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Standards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Country",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Country",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Branch",
                type: "int",
                maxLength: 64,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_CountryId",
                table: "Standards",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CountryId",
                table: "Branch",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Country_CountryId",
                table: "Branch",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Standards_Country_CountryId",
                table: "Standards",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Country_CountryId",
                table: "Branch");

            migrationBuilder.DropForeignKey(
                name: "FK_Standards_Country_CountryId",
                table: "Standards");

            migrationBuilder.DropIndex(
                name: "IX_Standards_CountryId",
                table: "Standards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropIndex(
                name: "IX_Branch_CountryId",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Standards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Branch");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Standards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CountryName",
                table: "Country",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "Branch",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "CountryName");

            migrationBuilder.InsertData(
                table: "Country",
                column: "CountryName",
                value: "Netherlands");

            migrationBuilder.CreateIndex(
                name: "IX_Standards_CountryName",
                table: "Standards",
                column: "CountryName");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CountryName",
                table: "Branch",
                column: "CountryName");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Country_CountryName",
                table: "Branch",
                column: "CountryName",
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
    }
}
