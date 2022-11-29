using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class update_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SurfaceAreaInM2",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 1,
                column: "SurfaceAreaInM2",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 2,
                column: "SurfaceAreaInM2",
                value: 140);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 3,
                column: "SurfaceAreaInM2",
                value: 80);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 4,
                column: "SurfaceAreaInM2",
                value: 200);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 5,
                column: "SurfaceAreaInM2",
                value: 150);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 6,
                column: "SurfaceAreaInM2",
                value: 180);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 7,
                column: "SurfaceAreaInM2",
                value: 90);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 8,
                column: "SurfaceAreaInM2",
                value: 100);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 9,
                column: "SurfaceAreaInM2",
                value: 70);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurfaceAreaInM2",
                table: "Department");
        }
    }
}
