using At.luki0606.DartZone.API.Models;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Data
{
    public class DartZoneDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DartZoneDbContext(DbContextOptions<DartZoneDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
