using Bumbodium.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata;


namespace Bumbodium.Data
{
    public class BumbodiumContext : DbContext
    {
        public DbSet<Presence> Presence { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Availability> Availability { get; set; }
        public DbSet<Shift> Shift { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Standards> Standards { get; set; }
        public DbSet<Forecast> Forecast { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployee { get; set; }
        public DbSet<BranchEmployee> BranchEmployee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=BumbodiumDB;Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
            .HasOne(a => a.Account)
            .WithOne(e => e.Employee)
            .HasForeignKey<Account>(a => a.EmployeeId);

            modelBuilder.Entity<BranchEmployee>()
                .HasKey(bp => new { bp.FiliaalId, bp.EmployeeId });

            modelBuilder.Entity<BranchEmployee>()
                .HasOne(bp => bp.Filiaal)
                .WithMany(p => p.PartOFEmployee)
                .HasForeignKey(bp => bp.FiliaalId);

            modelBuilder.Entity<BranchEmployee>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.PartOFFiliaal)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<DepartmentEmployee>()
                .HasKey(t => new { t.DepartmentId, t.EmployeeId });

            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(pt => pt.Department)
                .WithMany(p => p.PartOFEmployee)
                .HasForeignKey(pt => pt.DepartmentId);

            modelBuilder.Entity<DepartmentEmployee>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.PartOFDepartment)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<Shift>()
                .HasOne(pt => pt.Department)
                .WithMany(t => t.Shifts)
                .HasForeignKey(pt => pt.DepartmentId);

            modelBuilder.Entity<Shift>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.Shifts)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<Forecast>()
                .HasKey(t => new { t.Date, t.DepartmentId });


            #region seedData
            InsertBranchData(modelBuilder);
            InsertDepartmentData(modelBuilder);
            InsertStandardsData(modelBuilder);
            InsertEmployeeData(modelBuilder);
            InsertAccountData(modelBuilder);
        }

        private static void InsertStandardsData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Standards>().HasData(
                new Standards()
                {
                    Id = 1,
                    Subject = "Coli",
                    Value = 5,
                    Description = "aantal minuten per Coli uitladen.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 2,
                    Subject = "VakkenVullen",
                    Value = 30,
                    Description = "aantal minuten Vakken vullen per Coli.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 3,
                    Subject = "Kasiere",
                    Value = 30,
                    Description = "1 Kasiere per uur per aantal klanten.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 4,
                    Subject = "Medewerker",
                    Value = 100,
                    Description = "1 medewerker per customer per uur per aantal klanten.",
                    Country = Country.Netherlands
                },

                new Standards()
                {
                    Id = 5,
                    Subject = "Spiegelen",
                    Value = 30,
                    Description = "aantal seconde voor medewerker per customer per meter.",
                    Country = Country.Netherlands
                });
        }

        private static void InsertAccountData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                                new Account { EmployeeId = 1, Username = "j.vangeest@bumbodium.nl", Password = "jan4ever" }
                            );
        }

        private static void InsertEmployeeData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                            new Employee { EmployeeID = 1, FirstName = "Jan", MiddleName = "van", LastName = "Geest", Birthdate = new DateTime(1989, 10, 22), PhoneNumber = "+31 6 56927484", Email = "j.vangeest@bumbodium.nl", DateInService = new DateTime(2006, 05, 12), Type = TypeStaff.Manager }
                            );
        }

        private static void InsertDepartmentData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                            new Department() { Id = 1, BranchId = 1, SurfaceAreaInM2 = 50, Name = DepartmentType.Vegetables_Fruit, Description = "Vegetables_Fruit" },
                            new Department() { Id = 2, BranchId = 1, SurfaceAreaInM2 = 140, Name = DepartmentType.Meat, Description = "Meat" },
                            new Department() { Id = 3, BranchId = 1, SurfaceAreaInM2 = 80, Name = DepartmentType.Fish, Description = "Fish" },
                            new Department() { Id = 4, BranchId = 1, SurfaceAreaInM2 = 200, Name = DepartmentType.Cheese_Milk, Description = "Cheese_Milk" },
                            new Department() { Id = 5, BranchId = 1, SurfaceAreaInM2 = 150, Name = DepartmentType.Bread, Description = "Bread" },
                            new Department() { Id = 6, BranchId = 1, SurfaceAreaInM2 = 180, Name = DepartmentType.Cosmetics, Description = "Cosmetics" },
                            new Department() { Id = 7, BranchId = 1, SurfaceAreaInM2 = 90, Name = DepartmentType.Checkout, Description = "Checkout" },
                            new Department() { Id = 8, BranchId = 1, SurfaceAreaInM2 = 100, Name = DepartmentType.Stockroom, Description = "Stockroom" },
                            new Department() { Id = 9, BranchId = 1, SurfaceAreaInM2 = 70, Name = DepartmentType.InformationDesk, Description = "InformationDesk" });
        }

        private static void InsertBranchData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>().HasData(
                            new Branch() { Id = 1, City = "Den Bosch", Street = "01", HouseNumber = "1", PostalCode = "0000 AA", Country = Country.Netherlands });
        }
        #endregion
    }
}
