using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class init_database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Beschikbaarheids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beschikbaarheids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dienstens",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShiftStartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dienstens", x => x.ShiftId);
                });

            migrationBuilder.CreateTable(
                name: "Filiaals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Streed = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    HomeNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiaals", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Normeringens",
                columns: table => new
                {
                    Description = table.Column<string>(type: "nvarchar(1048)", maxLength: 1048, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Normeringens", x => x.Description);
                });

            migrationBuilder.CreateTable(
                name: "Presence",
                columns: table => new
                {
                    ClockInDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteredClockInDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlteredClockOutDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presence", x => x.ClockInDateTime);
                });

            migrationBuilder.CreateTable(
                name: "Prognoses",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountExpectedEmployees = table.Column<int>(type: "int", nullable: false),
                    AmountExpectedCustomers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prognoses", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "Werknemers",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ExtraName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DateInService = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOutService = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Function = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AvailabilityId = table.Column<int>(type: "int", nullable: true),
                    PresenceClockInDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShiftId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Werknemers", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Werknemers_Beschikbaarheids_AvailabilityId",
                        column: x => x.AvailabilityId,
                        principalTable: "Beschikbaarheids",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Werknemers_Dienstens_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Dienstens",
                        principalColumn: "ShiftId");
                    table.ForeignKey(
                        name: "FK_Werknemers_Presence_PresenceClockInDateTime",
                        column: x => x.PresenceClockInDateTime,
                        principalTable: "Presence",
                        principalColumn: "ClockInDateTime");
                });

            migrationBuilder.CreateTable(
                name: "Afdelings",
                columns: table => new
                {
                    Name = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ForecastDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShiftId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afdelings", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Afdelings_Dienstens_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Dienstens",
                        principalColumn: "ShiftId");
                    table.ForeignKey(
                        name: "FK_Afdelings_Prognoses_ForecastDate",
                        column: x => x.ForecastDate,
                        principalTable: "Prognoses",
                        principalColumn: "Date");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Afdelings_ForecastDate",
                table: "Afdelings",
                column: "ForecastDate");

            migrationBuilder.CreateIndex(
                name: "IX_Afdelings_ShiftId",
                table: "Afdelings",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_AvailabilityId",
                table: "Werknemers",
                column: "AvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_PresenceClockInDateTime",
                table: "Werknemers",
                column: "PresenceClockInDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_ShiftId",
                table: "Werknemers",
                column: "ShiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Afdelings");

            migrationBuilder.DropTable(
                name: "Filiaals");

            migrationBuilder.DropTable(
                name: "Normeringens");

            migrationBuilder.DropTable(
                name: "Werknemers");

            migrationBuilder.DropTable(
                name: "Prognoses");

            migrationBuilder.DropTable(
                name: "Beschikbaarheids");

            migrationBuilder.DropTable(
                name: "Dienstens");

            migrationBuilder.DropTable(
                name: "Presence");
        }
    }
}
