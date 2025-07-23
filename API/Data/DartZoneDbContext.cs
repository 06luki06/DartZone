using At.luki0606.DartZone.API.Models;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Data
{
    public class DartZoneDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Throw> Throws => Set<Throw>();
        public DbSet<Dart> Darts => Set<Dart>();

        public DartZoneDbContext(DbContextOptions<DartZoneDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.PasswordSalt).IsRequired();
            });
            #endregion

            #region Game Configuration
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.StartScore).IsRequired();
                entity.Property(g => g.CurrentScore).IsRequired();
                entity.Property(g => g.CreatedAt).IsRequired();
                entity.Property(g => g.HasFinished).IsRequired();
                entity.Property(g => g.HasStarted).IsRequired();

                entity.HasOne(g => g.User)
                      .WithMany() // Du hast keine ICollection<Game> im User definiert
                      .HasForeignKey(g => g.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(g => g.Throws)
                      .WithOne(t => t.Game)
                      .HasForeignKey(t => t.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Throw Configuration
            modelBuilder.Entity<Throw>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.CreatedAt).IsRequired();

                entity.HasOne(t => t.Game)
                      .WithMany(g => g.Throws)
                      .HasForeignKey(t => t.GameId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Darts)
                      .WithOne(d => d.Throw)
                      .HasForeignKey(d => d.ThrowId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region Dart Configuration
            modelBuilder.Entity<Dart>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Field).IsRequired();
                entity.Property(d => d.Multiplier).IsRequired();

                entity.HasOne(d => d.Throw)
                      .WithMany(t => t.Darts)
                      .HasForeignKey(d => d.ThrowId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
        }
    }
}
