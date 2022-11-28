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
        public DbSet<Country> Country { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string your_password = "BumboAdmin!";
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer($"Server=tcp:bumbodium.database.windows.net,1433;Initial Catalog=BumbodiumDB;Persist Security Info=False;User ID=CloudSAc92745b1;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
                .HasKey(t => new { t.DepartmentId, t.EmployeeId, t.ShiftId });

            modelBuilder.Entity<Shift>()
                .HasOne(pt => pt.Department)
                .WithMany(t => t.Shifts)
                .HasForeignKey(pt => pt.DepartmentId);

            modelBuilder.Entity<Shift>()
                .HasOne(pt => pt.Employee)
                .WithMany(t => t.Shifts)
                .HasForeignKey(pt => pt.EmployeeId);

            modelBuilder.Entity<Availability>()
                .HasKey(t => new { t.AvailabilityId, t.EmployeeId });

            modelBuilder.Entity<Presence>()
                .HasKey(t => new { t.PresenceId, t.EmployeeId });

            modelBuilder.Entity<Forecast>()
                .HasKey(t => new { t.Date, t.DepartmentId });

            #region seedData
            /*InsertCountyData(modelBuilder);
            InsertBranchData(modelBuilder);
            InsertDepartmentData(modelBuilder);
            //InsertEmployeeData(modelBuilder);
            //InsertAccountData(modelBuilder);
            InsertStandardsData(modelBuilder);*/

            #endregion
        }

        private static void InsertStandardsData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Standards>().HasData(
                new Standards()
                {
                    Id = "Coli",
                    Value = 5,
                    Description = "aantal minuten per Coli uitladen.",
                    CountryId = 1
                },

                new Standards()
                {
                    Id = "VakkenVullen",
                    Value = 30,
                    Description = "aantal minuten Vakken vullen per Coli.",
                    CountryId = 1
                },

                new Standards()
                {
                    Id = "Kasiere",
                    Value = 30,
                    Description = "1 Kasiere per uur per aantal klanten.",
                    CountryId = 1
                },

                new Standards()
                {
                    Id = "Medewerker",
                    Value = 100,
                    Description = "1 medewerker per customer per uur per aantal klanten.",
                    CountryId = 1
                },

                new Standards()
                {
                    Id = "Spiegelen",
                    Value = 30,
                    Description = "aantal seconde voor medewerker per customer per meter.",
                    CountryId = 1
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
                            new Department() { BranchId = 1, Name = DepartmentType.Vegetables_Fruit, Description = "Vegetables_Fruit" },
                            new Department() { BranchId = 1, Name = DepartmentType.Meat, Description = "Meat" },
                            new Department() { BranchId = 1, Name = DepartmentType.Fish, Description = "Fish" },
                            new Department() { BranchId = 1, Name = DepartmentType.Cheese_Milk, Description = "Cheese_Milk" },
                            new Department() { BranchId = 1, Name = DepartmentType.Bread, Description = "Bread" },
                            new Department() { BranchId = 1, Name = DepartmentType.Cosmetics, Description = "Cosmetics" },
                            new Department() { BranchId = 1, Name = DepartmentType.Checkout, Description = "Checkout" },
                            new Department() { BranchId = 1, Name = DepartmentType.Stockroom, Description = "Stockroom" },
                            new Department() { BranchId = 1, Name = DepartmentType.InformationDesk, Description = "InformationDesk" });
        }

        private static void InsertCountyData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country() { Id = 1, CountryName = "Netherlands" });
        }

        private static void InsertBranchData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>().HasData(
                            new Branch() { Id = 1, City = "Den Bosch", Street = "01", HouseNumber = "1", PostalCode = "0000 AA", CountryId = 1 });
        }
    }
}
