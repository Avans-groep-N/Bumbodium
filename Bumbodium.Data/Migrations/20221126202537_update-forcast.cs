using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class updateforcast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecast_Department_DepartmentName",
                table: "Forecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecast",
                table: "Forecast");

            migrationBuilder.DropIndex(
                name: "IX_Forecast_DepartmentName",
                table: "Forecast");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Forecast");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Forecast",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecast",
                table: "Forecast",
                columns: new[] { "Date", "DepartmentId" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forecast_Department_DepartmentId",
                table: "Forecast");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Forecast",
                table: "Forecast");

            migrationBuilder.DropIndex(
                name: "IX_Forecast_DepartmentId",
                table: "Forecast");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Forecast");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentName",
                table: "Forecast",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forecast",
                table: "Forecast",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Forecast_DepartmentName",
                table: "Forecast",
                column: "DepartmentName");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecast_Department_DepartmentName",
                table: "Forecast",
                column: "DepartmentName",
                principalTable: "Department",
                principalColumn: "Name");
        }
    }
}
