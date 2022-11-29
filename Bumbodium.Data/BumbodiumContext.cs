using Bumbodium.Data.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata;


namespace Bumbodium.Data
{
    public class BumbodiumContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public BumbodiumContext(DbContextOptions<BumbodiumContext> options)
            : base(options)
        {
        }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Availability>()
                .HasKey(t => new { t.AvailabilityId, t.EmployeeId});

            modelBuilder.Entity<Presence>()
                .HasKey(t => new { t.PresenceId, t.EmployeeId });
        }
    }
}
