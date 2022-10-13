using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    public partial class init_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aanwezigheids",
                columns: table => new
                {
                    InklokDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UitklokDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AangepasteInklokDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AangepasteUitklokDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aanwezigheids", x => x.InklokDatumTijd);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Wachtwoord = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
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
                    BeginDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EindDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DienstBeginDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DienstEindDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dienstens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filiaals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stad = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Huisnummer = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Land = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiaals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Normeringens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Beschrijving = table.Column<string>(type: "nvarchar(1048)", maxLength: 1048, nullable: false),
                    Waarde = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Normeringens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prognoses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AantalVerwachtteWerknemers = table.Column<int>(type: "int", nullable: false),
                    AantalVerwachtteKlanten = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prognoses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Werknemers",
                columns: table => new
                {
                    WerknemerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TussenVoegsel = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Achternaam = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Geboortedatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telefoonnummer = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DatumInDienst = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumUitDienst = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Functie = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    AanwezigheidInklokDatumTijd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BeschikbaarheidId = table.Column<int>(type: "int", nullable: true),
                    DienstenId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Werknemers", x => x.WerknemerId);
                    table.ForeignKey(
                        name: "FK_Werknemers_Aanwezigheids_AanwezigheidInklokDatumTijd",
                        column: x => x.AanwezigheidInklokDatumTijd,
                        principalTable: "Aanwezigheids",
                        principalColumn: "InklokDatumTijd");
                    table.ForeignKey(
                        name: "FK_Werknemers_Beschikbaarheids_BeschikbaarheidId",
                        column: x => x.BeschikbaarheidId,
                        principalTable: "Beschikbaarheids",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Werknemers_Dienstens_DienstenId",
                        column: x => x.DienstenId,
                        principalTable: "Dienstens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Afdelings",
                columns: table => new
                {
                    Naam = table.Column<int>(type: "int", nullable: false),
                    Beschrijving = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DienstenId = table.Column<int>(type: "int", nullable: true),
                    PrognoseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Afdelings", x => x.Naam);
                    table.ForeignKey(
                        name: "FK_Afdelings_Dienstens_DienstenId",
                        column: x => x.DienstenId,
                        principalTable: "Dienstens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Afdelings_Prognoses_PrognoseId",
                        column: x => x.PrognoseId,
                        principalTable: "Prognoses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Afdelings_DienstenId",
                table: "Afdelings",
                column: "DienstenId");

            migrationBuilder.CreateIndex(
                name: "IX_Afdelings_PrognoseId",
                table: "Afdelings",
                column: "PrognoseId");

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_AanwezigheidInklokDatumTijd",
                table: "Werknemers",
                column: "AanwezigheidInklokDatumTijd");

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_BeschikbaarheidId",
                table: "Werknemers",
                column: "BeschikbaarheidId");

            migrationBuilder.CreateIndex(
                name: "IX_Werknemers_DienstenId",
                table: "Werknemers",
                column: "DienstenId");
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
                name: "Aanwezigheids");

            migrationBuilder.DropTable(
                name: "Beschikbaarheids");

            migrationBuilder.DropTable(
                name: "Dienstens");
        }
    }
}
