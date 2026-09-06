using DailyTaskTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DailyTaskTracker.Application.Interfaces;

/// <summary>
/// Abstraction interface for the EF Core DbContext used by the Application Layer.
/// Decouples Application logic from direct Infrastructure database dependencies.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<TaskItem> Tasks { get; }
    DbSet<TaskCompletion> TaskCompletions { get; }
    DbSet<Habit> Habits { get; }
    DbSet<HabitCompletion> HabitCompletions { get; }
    DbSet<Category> Categories { get; }
    DbSet<User> Users { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
