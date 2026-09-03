

using Microsoft.EntityFrameworkCore;
using Planara.Notifications.Data.Domain;

namespace Planara.Notifications.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<NotificationAttempt> NotificationAttempts { get; set; } = null!;
    public DbSet<NotificationDelivery> NotificationDeliveries { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<NotificationDelivery>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<NotificationDelivery>()
            .HasIndex(x => x.EventId)
            .IsUnique();

        modelBuilder.Entity<NotificationDelivery>()
            .Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<NotificationDelivery>()
            .Property(x => x.Channel)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<NotificationDelivery>()
            .Property(x => x.Recipient)
            .HasMaxLength(320)
            .IsRequired();

        modelBuilder.Entity<NotificationDelivery>()
            .Property(x => x.PayloadJson)
            .HasColumnType("jsonb")
            .IsRequired();

        modelBuilder.Entity<NotificationDelivery>()
            .Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<NotificationDelivery>()
            .Property(x => x.AttemptCount)
            .HasDefaultValue(0)
            .IsRequired();

        modelBuilder.Entity<NotificationDelivery>()
            .HasMany(x => x.Attempts)
            .WithOne(x => x.NotificationDelivery)
            .HasForeignKey(x => x.NotificationDeliveryId)
            .OnDelete(DeleteBehavior.Cascade);
                
        modelBuilder.Entity<NotificationDelivery>()
            .HasIndex(x => new { x.Status, x.NextAttemptAt });

        modelBuilder.Entity<NotificationAttempt>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<NotificationAttempt>()
            .HasIndex(x => x.NotificationDeliveryId);

        modelBuilder.Entity<NotificationAttempt>()
            .Property(x => x.StartedAt)
            .IsRequired();

        modelBuilder.Entity<NotificationAttempt>()
            .Property(x => x.Error)
            .HasMaxLength(4000);
    }
}