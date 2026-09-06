using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Habits;

public record HabitResponse(
    Guid Id,
    string Name,
    string Description,
    HabitFrequency Frequency,
    int TargetPerPeriod,
    int CurrentStreak,
    int LongestStreak,
    double CompletionRate,
    bool IsCompletedToday,
    DateTime CreatedAtUtc,
    List<DateOnly> CompletionDates
);
