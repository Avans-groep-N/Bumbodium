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
                optionsBuilder.UseSqlServer("Server=localhost;Database=BumbodiumDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
            .HasOne(a => a.Account)
            .WithOne(e => e.Employee)
            .HasForeignKey<Account>(a => a.EmployeeId);

            modelBuilder.Entity<BranchEmployee>()
                .HasKey(t => new { t.FiliaalId, t.EmployeeId });

            modelBuilder.Entity<BranchEmployee>()
                .HasOne(pt => pt.Filiaal)
                .WithMany(p => p.PartOFEmployee)
                .HasForeignKey(pt => pt.FiliaalId);

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

            /*modelBuilder.Entity<Employee>().HasData(
                new Employee { EmployeeID = 1, FirstName = "Jan", ExtraName = "van", LastName = "Geest", Birthdate = new DateTime(1989, 10, 22), PhoneNumber = "+31 6 56927484", Email = "j.vangeest@bumbodium.nl", DateInService = new DateTime(2006, 05, 12), WorkFunction = "Manager" },
                new Employee { }
                );

            modelBuilder.Entity<Branch>().HasData(
                    new Branch { City = "'s-Hertogenbosch", Street = "Supermarktboulevard", PostalCode = "5220UL", HouseNumber = "5", Country = "Netherlands" }
                );

            modelBuilder.Entity<Account>().HasData(
                    new Account { EmployeeId = 1, Email = "j.vangeest@bumbodium.nl", Password = "jan4ever", Type = TypeStaff.Manager }
                );

            modelBuilder.Entity<Department>().HasData(
                    new Department { Name = AfdelingType.Groente, Description = "Vulrekken vers" }
                );

            modelBuilder.Entity<Availability>().HasData(
                    new Availability { Id = 1, StartDateTime = new DateTime(2022, 10, 17, 08, 00, 00), EndDateTime = new DateTime(2022, 10, 17, 13, 00, 00), Type = BeschikbaarheidType.Schoolhours }
                );*/
        }
    }
}
