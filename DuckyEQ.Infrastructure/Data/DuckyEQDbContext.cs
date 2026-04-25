using DuckyEQ.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Data;

public class DuckyEQDbContext : DbContext
{
    public DuckyEQDbContext(DbContextOptions<DuckyEQDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PillarProgress> PillarProgresses => Set<PillarProgress>();
    public DbSet<UserLessonProgress> UserLessonProgresses => Set<UserLessonProgress>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<LessonConnectQuestion> LessonConnectQuestions => Set<LessonConnectQuestion>();
    public DbSet<LessonReflectQuestion> LessonReflectQuestions => Set<LessonReflectQuestion>();
    public DbSet<EQTestQuestion> EQTestQuestions => Set<EQTestQuestion>();
    public DbSet<UserEQTestResult> UserEQTestResults => Set<UserEQTestResult>();
    public DbSet<QuackCoins> QuackCoins => Set<QuackCoins>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();
    public DbSet<UserInventory> UserInventories => Set<UserInventory>();
    public DbSet<GratitudeEntry> GratitudeEntries => Set<GratitudeEntry>();
    public DbSet<DailyCheckIn> DailyCheckIns => Set<DailyCheckIn>();
    public DbSet<Friendship> Friendships => Set<Friendship>();
    public DbSet<Quack> Quacks => Set<Quack>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── User ──────────────────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Email).IsRequired().HasMaxLength(256);
            e.Property(u => u.Username).IsRequired().HasMaxLength(50);
            e.Property(u => u.KnownAs).IsRequired().HasMaxLength(10);
            e.Property(u => u.PasswordHash).IsRequired();
            e.HasIndex(u => u.Username).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
        });

        // ── PillarProgress ────────────────────────────────────────────────
        modelBuilder.Entity<PillarProgress>(e =>
        {
            e.HasKey(p => p.Id);
        });

        // ── UserLessonProgress ────────────────────────────────────────────
        modelBuilder.Entity<UserLessonProgress>(e =>
        {
            e.HasKey(p => p.Id);
        });

        // ── Lesson ────────────────────────────────────────────────────────
        modelBuilder.Entity<Lesson>(e =>
        {
            e.HasKey(l => l.Id);
            e.Property(l => l.DefineFlashcardsJson).HasColumnType("nvarchar(max)");
            e.Property(l => l.EngageConfigJson).HasColumnType("nvarchar(max)");
            e.Property(l => l.DuckArcJson).HasColumnType("nvarchar(max)");
            e.Property(l => l.ShareCardConfigJson).HasColumnType("nvarchar(max)");
        });

        // ── LessonConnectQuestion ─────────────────────────────────────────
        modelBuilder.Entity<LessonConnectQuestion>(e =>
        {
            e.HasKey(q => q.Id);
        });

        // ── LessonReflectQuestion ─────────────────────────────────────────
        modelBuilder.Entity<LessonReflectQuestion>(e =>
        {
            e.HasKey(q => q.Id);
        });

        // ── EQTestQuestion ────────────────────────────────────────────────
        modelBuilder.Entity<EQTestQuestion>(e =>
        {
            e.HasKey(q => q.Id);
            e.Property(q => q.Explanation).HasColumnType("nvarchar(max)");
        });

        // ── UserEQTestResult ──────────────────────────────────────────────
        modelBuilder.Entity<UserEQTestResult>(e =>
        {
            e.HasKey(r => r.Id);
        });

        // ── QuackCoins ────────────────────────────────────────────────────
        modelBuilder.Entity<QuackCoins>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasIndex(c => c.UserId).IsUnique();
        });

        // ── ShopItem ──────────────────────────────────────────────────────
        modelBuilder.Entity<ShopItem>(e =>
        {
            e.HasKey(s => s.Id);
            e.Property(s => s.Name).IsRequired().HasMaxLength(100);
            e.Property(s => s.Description).HasMaxLength(500);
            e.Property(s => s.Rarity).HasMaxLength(50);
        });

        // ── UserInventory ─────────────────────────────────────────────────
        modelBuilder.Entity<UserInventory>(e =>
        {
            e.HasKey(i => i.Id);
        });

        // ── GratitudeEntry ────────────────────────────────────────────────
        modelBuilder.Entity<GratitudeEntry>(e =>
        {
            e.HasKey(g => g.Id);
            e.Property(g => g.Text).IsRequired().HasMaxLength(500);
        });

        // ── DailyCheckIn ──────────────────────────────────────────────────
        modelBuilder.Entity<DailyCheckIn>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Phrase).HasMaxLength(120);
            // Unique: one check-in per user per calendar day
            e.HasIndex(c => new { c.UserId, c.CheckInDate }).IsUnique();
        });

        // ── Friendship ────────────────────────────────────────────────────
        // Two FKs to User — must use DeleteBehavior.Restrict on both
        // to avoid SQL Server cascade cycle error
        modelBuilder.Entity<Friendship>(e =>
        {
            e.HasKey(f => f.Id);

            e.HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<User>()
                .WithMany()
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(f => new { f.RequesterId, f.AddresseeId }).IsUnique();
        });

        // ── Quack ─────────────────────────────────────────────────────────
        // Two FKs to User — same Restrict treatment as Friendship
        modelBuilder.Entity<Quack>(e =>
        {
            e.HasKey(q => q.Id);

            e.HasOne<User>()
                .WithMany()
                .HasForeignKey(q => q.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<User>()
                .WithMany()
                .HasForeignKey(q => q.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
