﻿// <auto-generated />
using System;
using Bumbodium.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bumbodium.Data.Migrations
{
    [DbContext(typeof(BumbodiumContext))]
    [Migration("20221201135033_db_init")]
    partial class db_init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bumbodium.Data.DBModels.Account", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            EmployeeId = 1,
                            Password = "jan4ever",
                            Username = "j.vangeest@bumbodium.nl"
                        });
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Availability", b =>
                {
                    b.Property<int>("AvailablityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AvailablityId"), 1L, 1);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("AvailablityId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Availability");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Country")
                        .HasColumnType("int");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.ToTable("Branch");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Den Bosch",
                            Country = 0,
                            HouseNumber = "1",
                            PostalCode = "0000 AA",
                            Street = "01"
                        });
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.BranchEmployee", b =>
                {
                    b.Property<int>("FiliaalId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("FiliaalId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("BranchEmployee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<int>("SurfaceAreaInM2")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.ToTable("Department");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BranchId = 1,
                            Description = "Vegetables_Fruit",
                            Name = 0,
                            SurfaceAreaInM2 = 50
                        },
                        new
                        {
                            Id = 2,
                            BranchId = 1,
                            Description = "Meat",
                            Name = 1,
                            SurfaceAreaInM2 = 140
                        },
                        new
                        {
                            Id = 3,
                            BranchId = 1,
                            Description = "Fish",
                            Name = 2,
                            SurfaceAreaInM2 = 80
                        },
                        new
                        {
                            Id = 4,
                            BranchId = 1,
                            Description = "Cheese_Milk",
                            Name = 3,
                            SurfaceAreaInM2 = 200
                        },
                        new
                        {
                            Id = 5,
                            BranchId = 1,
                            Description = "Bread",
                            Name = 4,
                            SurfaceAreaInM2 = 150
                        },
                        new
                        {
                            Id = 6,
                            BranchId = 1,
                            Description = "Cosmetics",
                            Name = 5,
                            SurfaceAreaInM2 = 180
                        },
                        new
                        {
                            Id = 7,
                            BranchId = 1,
                            Description = "Checkout",
                            Name = 6,
                            SurfaceAreaInM2 = 90
                        },
                        new
                        {
                            Id = 8,
                            BranchId = 1,
                            Description = "Stockroom",
                            Name = 7,
                            SurfaceAreaInM2 = 100
                        },
                        new
                        {
                            Id = 9,
                            BranchId = 1,
                            Description = "InformationDesk",
                            Name = 8,
                            SurfaceAreaInM2 = 70
                        });
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.DepartmentEmployee", b =>
                {
                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("WorkFunction")
                        .HasColumnType("int");

                    b.HasKey("DepartmentId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("DepartmentEmployee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"), 1L, 1);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateInService")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOutService")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            EmployeeID = 1,
                            Birthdate = new DateTime(1989, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateInService = new DateTime(2006, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "j.vangeest@bumbodium.nl",
                            FirstName = "Jan",
                            LastName = "Geest",
                            MiddleName = "van",
                            PhoneNumber = "+31 6 56927484",
                            Type = 0
                        });
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Forecast", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("AmountExpectedColis")
                        .HasColumnType("int");

                    b.Property<int>("AmountExpectedCustomers")
                        .HasColumnType("int");

                    b.Property<int>("AmountExpectedEmployees")
                        .HasColumnType("int");

                    b.Property<int?>("StandardsId")
                        .HasColumnType("int");

                    b.HasKey("Date", "DepartmentId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("StandardsId");

                    b.ToTable("Forecast");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Presence", b =>
                {
                    b.Property<int>("PresenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PresenceId"), 1L, 1);

                    b.Property<DateTime?>("AlteredClockInDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("AlteredClockOutDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ClockInDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ClockOutDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("PresenceId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Presence");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Shift", b =>
                {
                    b.Property<int>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShiftId"), 1L, 1);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShiftEndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShiftStartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ShiftId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Shift");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Standards", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Country")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1048)
                        .HasColumnType("nvarchar(1048)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Standards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = 0,
                            Description = "aantal minuten per Coli uitladen.",
                            Subject = "Coli",
                            Value = 5
                        },
                        new
                        {
                            Id = 2,
                            Country = 0,
                            Description = "aantal minuten Vakken vullen per Coli.",
                            Subject = "VakkenVullen",
                            Value = 30
                        },
                        new
                        {
                            Id = 3,
                            Country = 0,
                            Description = "1 Kasiere per uur per aantal klanten.",
                            Subject = "Kasiere",
                            Value = 30
                        },
                        new
                        {
                            Id = 4,
                            Country = 0,
                            Description = "1 medewerker per customer per uur per aantal klanten.",
                            Subject = "Medewerker",
                            Value = 100
                        },
                        new
                        {
                            Id = 5,
                            Country = 0,
                            Description = "aantal seconde voor medewerker per customer per meter.",
                            Subject = "Spiegelen",
                            Value = 30
                        });
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Account", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Employee", "Employee")
                        .WithOne("Account")
                        .HasForeignKey("Bumbodium.Data.DBModels.Account", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Availability", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Employee", "Employee")
                        .WithMany("Availability")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.BranchEmployee", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Employee", "Employee")
                        .WithMany("PartOFFiliaal")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bumbodium.Data.DBModels.Branch", "Filiaal")
                        .WithMany("PartOFEmployee")
                        .HasForeignKey("FiliaalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Filiaal");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Department", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.DepartmentEmployee", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Department", "Department")
                        .WithMany("PartOFEmployee")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bumbodium.Data.DBModels.Employee", "Employee")
                        .WithMany("PartOFDepartment")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Forecast", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Department", "Department")
                        .WithMany("Forecast")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bumbodium.Data.DBModels.Standards", null)
                        .WithMany("ForecastId")
                        .HasForeignKey("StandardsId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Presence", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Employee", "Employee")
                        .WithMany("Presence")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Shift", b =>
                {
                    b.HasOne("Bumbodium.Data.DBModels.Department", "Department")
                        .WithMany("Shifts")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bumbodium.Data.DBModels.Employee", "Employee")
                        .WithMany("Shifts")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Branch", b =>
                {
                    b.Navigation("PartOFEmployee");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Department", b =>
                {
                    b.Navigation("Forecast");

                    b.Navigation("PartOFEmployee");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Employee", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("Availability");

                    b.Navigation("PartOFDepartment");

                    b.Navigation("PartOFFiliaal");

                    b.Navigation("Presence");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("Bumbodium.Data.DBModels.Standards", b =>
                {
                    b.Navigation("ForecastId");
                });
#pragma warning restore 612, 618
        }
    }
}