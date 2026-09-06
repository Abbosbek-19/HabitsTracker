namespace DailyTaskTracker.Application.DTOs.Statistics;

public record WeeklyDayStat(
    string DayOfWeek,
    int TotalTasks,
    int CompletedTasks,
    double CompletionRate
);

public record StatisticsResponse(
    int TotalTasks,
    int CompletedTasks,
    int IncompleteTasks,
    double CompletionRate,
    int ActiveHabitStreaks,
    string? BestDay,
    string? WorstDay,
    List<WeeklyDayStat> WeeklyBreakdown
);
