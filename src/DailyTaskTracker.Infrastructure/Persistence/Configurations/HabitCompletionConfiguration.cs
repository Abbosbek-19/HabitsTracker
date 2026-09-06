using DailyTaskTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyTaskTracker.Infrastructure.Persistence.Configurations;

public class HabitCompletionConfiguration : IEntityTypeConfiguration<HabitCompletion>
{
    public void Configure(EntityTypeBuilder<HabitCompletion> builder)
    {
        builder.ToTable("HabitCompletions");

        builder.HasKey(hc => hc.Id);

        builder.Property(hc => hc.CompletedDate)
            .IsRequired();

        builder.HasOne(hc => hc.Habit)
            .WithMany(h => h.Completions)
            .HasForeignKey(hc => hc.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(hc => new { hc.HabitId, hc.CompletedDate }).IsUnique();
    }
}
