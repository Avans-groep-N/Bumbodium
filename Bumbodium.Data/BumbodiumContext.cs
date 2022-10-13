using Microsoft.EntityFrameworkCore;


namespace Bumbodium.Data
{
    public class BumbodiumContext : DbContext
    {
        public DbSet<Aanwezigheid> Aanwezigheids { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Afdeling> Afdelings { get; set; }
        public DbSet<Beschikbaarheid> Beschikbaarheids { get; set; }
        public DbSet<Diensten> Dienstens { get; set; }
        public DbSet<Filiaal> Filiaals { get; set; }
        public DbSet<Normeringen> Normeringens { get; set; }
        public DbSet<Prognose> Prognoses { get; set; }
        public DbSet<Werknemer> Werknemers { get; set; }

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
