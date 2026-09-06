using DailyTaskTracker.Application.DTOs.Tasks;

namespace DailyTaskTracker.Application.DTOs.Calendar;

public record CalendarDayResponse(
    DateOnly Date,
    int CompletedTaskCount,
    int TotalTaskCount,
    double CompletionRate,
    List<TaskResponse> Tasks
);

public record CalendarMonthResponse(
    int Year,
    int Month,
    string MonthName,
    List<CalendarDayResponse> Days
);
