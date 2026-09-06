using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Habits;

public record CreateHabitRequest(
    string Name,
    string Description,
    HabitFrequency Frequency = HabitFrequency.Daily,
    int TargetPerPeriod = 1
);
