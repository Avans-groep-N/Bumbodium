using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class db_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DateInService = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOutService = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Standards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1048)", maxLength: 1048, nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Accounts_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    AvailabilityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => new { x.AvailabilityId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_Availability_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchEmployee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FiliaalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchEmployee", x => new { x.FiliaalId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_BranchEmployee_Branch_FiliaalId",
                        column: x => x.FiliaalId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BranchEmployee_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Presence",
                columns: table => new
                {
                    PresenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ClockInDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteredClockInDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AlteredClockOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presence", x => new { x.PresenceId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_Presence_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentEmployee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    WorkFunction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentEmployee", x => new { x.DepartmentId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_DepartmentEmployee_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentEmployee_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Forecast",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    AmountExpectedEmployees = table.Column<int>(type: "int", nullable: false),
                    AmountExpectedCustomers = table.Column<int>(type: "int", nullable: false),
                    AmountExpectedColis = table.Column<int>(type: "int", nullable: false),
                    StandardsId = table.Column<string>(type: "nvarchar(32)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forecast", x => new { x.Date, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_Forecast_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Forecast_Standards_StandardsId",
                        column: x => x.StandardsId,
                        principalTable: "Standards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ShiftStartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => new { x.DepartmentId, x.EmployeeId, x.ShiftId });
                    table.ForeignKey(
                        name: "FK_Shift_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shift_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "City", "Country", "HouseNumber", "PostalCode", "Street" },
                values: new object[] { 1, "Den Bosch", 0, "1", "0000 AA", "01" });

            migrationBuilder.InsertData(
                table: "Standards",
                columns: new[] { "Id", "Country", "Description", "Value" },
                values: new object[,]
                {
                    { "Coli", 0, "aantal minuten per Coli uitladen.", 5 },
                    { "Kasiere", 0, "1 Kasiere per uur per aantal klanten.", 30 },
                    { "Medewerker", 0, "1 medewerker per customer per uur per aantal klanten.", 100 },
                    { "Spiegelen", 0, "aantal seconde voor medewerker per customer per meter.", 30 },
                    { "VakkenVullen", 0, "aantal minuten Vakken vullen per Coli.", 30 }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "BranchId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Vegetables_Fruit", 0 },
                    { 2, 1, "Meat", 1 },
                    { 3, 1, "Fish", 2 },
                    { 4, 1, "Cheese_Milk", 3 },
                    { 5, 1, "Bread", 4 },
                    { 6, 1, "Cosmetics", 5 },
                    { 7, 1, "Checkout", 6 },
                    { 8, 1, "Stockroom", 7 },
                    { 9, 1, "InformationDesk", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Availability_EmployeeId",
                table: "Availability",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchEmployee_EmployeeId",
                table: "BranchEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_BranchId",
                table: "Department",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentEmployee_EmployeeId",
                table: "DepartmentEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Forecast_DepartmentId",
                table: "Forecast",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Forecast_StandardsId",
                table: "Forecast",
                column: "StandardsId");

            migrationBuilder.CreateIndex(
                name: "IX_Presence_EmployeeId",
                table: "Presence",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_EmployeeId",
                table: "Shift",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Availability");

            migrationBuilder.DropTable(
                name: "BranchEmployee");

            migrationBuilder.DropTable(
                name: "DepartmentEmployee");

            migrationBuilder.DropTable(
                name: "Forecast");

            migrationBuilder.DropTable(
                name: "Presence");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropTable(
                name: "Standards");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Branch");
        }
    }
}
