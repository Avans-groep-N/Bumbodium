using Microsoft.EntityFrameworkCore;


namespace Bumbodium.Data
{
    public class BumbodiumContext : DbContext
    {
        public DbSet<Presence> Presence { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Department> Afdelings { get; set; }
        public DbSet<Availability> Beschikbaarheids { get; set; }
        public DbSet<Shift> Dienstens { get; set; }
        public DbSet<Filiaal> Filiaals { get; set; }
        public DbSet<Standards> Normeringens { get; set; }
        public DbSet<Forecast> Prognoses { get; set; }
        public DbSet<Employee> Werknemers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=BumbodiumDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
