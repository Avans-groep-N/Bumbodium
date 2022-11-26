using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class Forecast_change_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecast_Department_DepartmentId",
                table: "Forecast");

            migrationBuilder.DropIndex(
                name: "IX_Forecast_DepartmentId",
                table: "Forecast");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Forecast_DepartmentId",
                table: "Forecast",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecast_Department_DepartmentId",
                table: "Forecast",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
