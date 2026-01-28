using GameTimeTracker.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTimeTracker.Data.Data;

public class GameTimeTrackerContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<PlaySession> PlaySessions { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    public GameTimeTrackerContext(DbContextOptions<GameTimeTrackerContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GameTimeTracker",
                "gametimetracker.db");

            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ProcessName);
            entity.Property(e => e.ProcessName).IsRequired();
            entity.Property(e => e.DisplayName).IsRequired();
            entity.Property(e => e.TotalPlaytime).HasConversion(
                v => v.Ticks,
                v => TimeSpan.FromTicks(v));
        });

        modelBuilder.Entity<PlaySession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.GameId);
            entity.HasIndex(e => e.StartTime);
            entity.Property(e => e.Duration).HasConversion(
                v => v.Ticks,
                v => TimeSpan.FromTicks(v));

            entity.HasOne(e => e.Game)
                .WithMany(g => g.PlaySessions)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MinimumDailyPlaytime).HasConversion(
                v => v.Ticks,
                v => TimeSpan.FromTicks(v));
            entity.Property(e => e.DailyReminderTime).HasConversion(
                v => v.HasValue ? v.Value.Ticks : (long?)null,
                v => v.HasValue ? TimeSpan.FromTicks(v.Value) : null);
        });
    }
}
