using DailyTaskTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyTaskTracker.Infrastructure.Persistence.Configurations;

public class TaskCompletionConfiguration : IEntityTypeConfiguration<TaskCompletion>
{
    public void Configure(EntityTypeBuilder<TaskCompletion> builder)
    {
        builder.ToTable("TaskCompletions");

        builder.HasKey(tc => tc.Id);

        builder.HasOne(tc => tc.Task)
            .WithMany(t => t.Completions)
            .HasForeignKey(tc => tc.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tc => new { tc.UserId, tc.CompletedAtUtc });
    }
}
