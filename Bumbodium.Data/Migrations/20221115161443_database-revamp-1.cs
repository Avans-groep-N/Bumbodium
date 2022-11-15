using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class databaserevamp1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchEmployee_Branch_FiliaalId",
                table: "BranchEmployee");

            migrationBuilder.RenameColumn(
                name: "FiliaalId",
                table: "BranchEmployee",
                newName: "BranchId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Branch",
                newName: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchEmployee_Branch_BranchId",
                table: "BranchEmployee",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchEmployee_Branch_BranchId",
                table: "BranchEmployee");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "BranchEmployee",
                newName: "FiliaalId");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Branch",
                newName: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchEmployee_Branch_FiliaalId",
                table: "BranchEmployee",
                column: "FiliaalId",
                principalTable: "Branch",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
