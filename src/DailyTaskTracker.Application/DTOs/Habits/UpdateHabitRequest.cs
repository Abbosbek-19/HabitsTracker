using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Habits;

public record UpdateHabitRequest(
    string Name,
    string Description,
    HabitFrequency Frequency,
    int TargetPerPeriod
);
