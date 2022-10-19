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
    [Migration("20221018145802_db-init-3")]
    partial class dbinit3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bumbodium.Data.Account", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Bumbodium.Data.Availability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Availability");
                });

            modelBuilder.Entity("Bumbodium.Data.Branch", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

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

                    b.HasKey("ID");

                    b.ToTable("Branch");
                });

            modelBuilder.Entity("Bumbodium.Data.BranchEmployee", b =>
                {
                    b.Property<int>("FiliaalId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("FiliaalId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("BranchEmployee");
                });

            modelBuilder.Entity("Bumbodium.Data.Department", b =>
                {
                    b.Property<int>("Name")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Name");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("Bumbodium.Data.DepartmentEmployee", b =>
                {
                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("DepartmentId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("DepartmentEmployee");
                });

            modelBuilder.Entity("Bumbodium.Data.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"), 1L, 1);

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateInService")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOutService")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("ExtraName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("WorkFunction")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.Forecast", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("AmountExpectedCustomers")
                        .HasColumnType("int");

                    b.Property<int>("AmountExpectedEmployees")
                        .HasColumnType("int");

                    b.Property<int?>("DepartmentName")
                        .HasColumnType("int");

                    b.Property<string>("StandardsDescription")
                        .HasColumnType("nvarchar(1048)");

                    b.HasKey("Date");

                    b.HasIndex("DepartmentName");

                    b.HasIndex("StandardsDescription");

                    b.ToTable("Forecast");
                });

            modelBuilder.Entity("Bumbodium.Data.Presence", b =>
                {
                    b.Property<DateTime>("ClockInDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("AlteredClockInDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("AlteredClockOutDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ClockOutDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.HasKey("ClockInDateTime");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Presence");
                });

            modelBuilder.Entity("Bumbodium.Data.Shift", b =>
                {
                    b.Property<int>("ShiftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShiftId"), 1L, 1);

                    b.Property<int?>("DepartmentName")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShiftEndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ShiftStartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ShiftId");

                    b.HasIndex("DepartmentName");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Shift");
                });

            modelBuilder.Entity("Bumbodium.Data.Standards", b =>
                {
                    b.Property<string>("Description")
                        .HasMaxLength(1048)
                        .HasColumnType("nvarchar(1048)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Description");

                    b.ToTable("Standards");
                });

            modelBuilder.Entity("Bumbodium.Data.Account", b =>
                {
                    b.HasOne("Bumbodium.Data.Employee", "Employee")
                        .WithOne("Account")
                        .HasForeignKey("Bumbodium.Data.Account", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.Availability", b =>
                {
                    b.HasOne("Bumbodium.Data.Employee", null)
                        .WithMany("Availability")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("Bumbodium.Data.BranchEmployee", b =>
                {
                    b.HasOne("Bumbodium.Data.Employee", "Employee")
                        .WithMany("PartOFFiliaal")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bumbodium.Data.Branch", "Filiaal")
                        .WithMany("PartOFEmployee")
                        .HasForeignKey("FiliaalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Filiaal");
                });

            modelBuilder.Entity("Bumbodium.Data.DepartmentEmployee", b =>
                {
                    b.HasOne("Bumbodium.Data.Department", "Department")
                        .WithMany("PartOFEmployee")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bumbodium.Data.Employee", "Employee")
                        .WithMany("PartOFDepartment")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("Bumbodium.Data.Forecast", b =>
                {
                    b.HasOne("Bumbodium.Data.Department", null)
                        .WithMany("ForecastId")
                        .HasForeignKey("DepartmentName");

                    b.HasOne("Bumbodium.Data.Standards", null)
                        .WithMany("ForecastId")
                        .HasForeignKey("StandardsDescription");
                });

            modelBuilder.Entity("Bumbodium.Data.Presence", b =>
                {
                    b.HasOne("Bumbodium.Data.Employee", null)
                        .WithMany("Presence")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("Bumbodium.Data.Shift", b =>
                {
                    b.HasOne("Bumbodium.Data.Department", null)
                        .WithMany("ShiftId")
                        .HasForeignKey("DepartmentName");

                    b.HasOne("Bumbodium.Data.Employee", null)
                        .WithMany("ShiftId")
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("Bumbodium.Data.Branch", b =>
                {
                    b.Navigation("PartOFEmployee");
                });

            modelBuilder.Entity("Bumbodium.Data.Department", b =>
                {
                    b.Navigation("ForecastId");

                    b.Navigation("PartOFEmployee");

                    b.Navigation("ShiftId");
                });

            modelBuilder.Entity("Bumbodium.Data.Employee", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();

                    b.Navigation("Availability");

                    b.Navigation("PartOFDepartment");

                    b.Navigation("PartOFFiliaal");

                    b.Navigation("Presence");

                    b.Navigation("ShiftId");
                });

            modelBuilder.Entity("Bumbodium.Data.Standards", b =>
                {
                    b.Navigation("ForecastId");
                });
#pragma warning restore 612, 618
        }
    }
}
