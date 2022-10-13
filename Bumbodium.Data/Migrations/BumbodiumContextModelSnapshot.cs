﻿// <auto-generated />
using System;
using Bumbodium.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    [DbContext(typeof(BumbodiumContext))]
    partial class BumbodiumContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bumbodium.Data.Aanwezigheid", b =>
                {
                    b.Property<DateTime>("InklokDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("AangepasteInklokDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("AangepasteUitklokDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UitklokDatumTijd")
                        .HasColumnType("datetime2");

                    b.HasKey("InklokDatumTijd");

                    b.ToTable("Aanwezigheids");
                });

            modelBuilder.Entity("Bumbodium.Data.Account", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Wachtwoord")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Email");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Bumbodium.Data.Afdeling", b =>
                {
                    b.Property<int>("Naam")
                        .HasColumnType("int");

                    b.Property<string>("Beschrijving")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("DienstenId")
                        .HasColumnType("int");

                    b.Property<int?>("PrognoseId")
                        .HasColumnType("int");

                    b.HasKey("Naam");

                    b.HasIndex("DienstenId");

                    b.HasIndex("PrognoseId");

                    b.ToTable("Afdelings");
                });

            modelBuilder.Entity("Bumbodium.Data.Beschikbaarheid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BeginDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EindDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Beschikbaarheids");
                });

            modelBuilder.Entity("Bumbodium.Data.Diensten", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DienstBeginDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DienstEindDatumTijd")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Dienstens");
                });

            modelBuilder.Entity("Bumbodium.Data.Filiaal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Huisnummer")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Land")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Stad")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Straat")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Filiaals");
                });

            modelBuilder.Entity("Bumbodium.Data.Normeringen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Beschrijving")
                        .IsRequired()
                        .HasMaxLength(1048)
                        .HasColumnType("nvarchar(1048)");

                    b.Property<int>("Waarde")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Normeringens");
                });

            modelBuilder.Entity("Bumbodium.Data.Prognose", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AantalVerwachtteKlanten")
                        .HasColumnType("int");

                    b.Property<int>("AantalVerwachtteWerknemers")
                        .HasColumnType("int");

                    b.Property<DateTime>("Datum")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Prognoses");
                });

            modelBuilder.Entity("Bumbodium.Data.Werknemer", b =>
                {
                    b.Property<int>("WerknemerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WerknemerId"), 1L, 1);

                    b.Property<DateTime?>("AanwezigheidInklokDatumTijd")
                        .HasColumnType("datetime2");

                    b.Property<string>("Achternaam")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int?>("BeschikbaarheidId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DatumInDienst")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DatumUitDienst")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DienstenId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Functie")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("Geboortedatum")
                        .HasColumnType("datetime2");

                    b.Property<string>("Telefoonnummer")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("TussenVoegsel")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Voornaam")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("WerknemerId");

                    b.HasIndex("AanwezigheidInklokDatumTijd");

                    b.HasIndex("BeschikbaarheidId");

                    b.HasIndex("DienstenId");

                    b.ToTable("Werknemers");
                });

            modelBuilder.Entity("Bumbodium.Data.Afdeling", b =>
                {
                    b.HasOne("Bumbodium.Data.Diensten", null)
                        .WithMany("AfdelingNaam")
                        .HasForeignKey("DienstenId");

                    b.HasOne("Bumbodium.Data.Prognose", null)
                        .WithMany("Afdelingen")
                        .HasForeignKey("PrognoseId");
                });

            modelBuilder.Entity("Bumbodium.Data.Werknemer", b =>
                {
                    b.HasOne("Bumbodium.Data.Aanwezigheid", null)
                        .WithMany("Werknemer")
                        .HasForeignKey("AanwezigheidInklokDatumTijd");

                    b.HasOne("Bumbodium.Data.Beschikbaarheid", null)
                        .WithMany("Werknemer")
                        .HasForeignKey("BeschikbaarheidId");

                    b.HasOne("Bumbodium.Data.Diensten", null)
                        .WithMany("Werknemer")
                        .HasForeignKey("DienstenId");
                });

            modelBuilder.Entity("Bumbodium.Data.Aanwezigheid", b =>
                {
                    b.Navigation("Werknemer");
                });

            modelBuilder.Entity("Bumbodium.Data.Beschikbaarheid", b =>
                {
                    b.Navigation("Werknemer");
                });

            modelBuilder.Entity("Bumbodium.Data.Diensten", b =>
                {
                    b.Navigation("AfdelingNaam");

                    b.Navigation("Werknemer");
                });

            modelBuilder.Entity("Bumbodium.Data.Prognose", b =>
                {
                    b.Navigation("Afdelingen");
                });
#pragma warning restore 612, 618
        }
    }
}
