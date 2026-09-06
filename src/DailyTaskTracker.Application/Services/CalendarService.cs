using System.Globalization;
using DailyTaskTracker.Application.DTOs.Calendar;
using DailyTaskTracker.Application.DTOs.Tasks;
using DailyTaskTracker.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DailyTaskTracker.Application.Services;

public class CalendarService : ICalendarService
{
    private readonly IApplicationDbContext _context;

    public CalendarService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CalendarMonthResponse> GetMonthlyCalendarAsync(int year, int month, Guid userId)
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);
        var startDate = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = new DateTime(year, month, daysInMonth, 23, 59, 59, DateTimeKind.Utc);

        var tasks = await _context.Tasks.AsNoTracking()
            .Where(t => (t.UserId == userId || userId == Guid.Empty) &&
                        t.DueDate.HasValue &&
                        t.DueDate.Value >= startDate &&
                        t.DueDate.Value <= endDate)
            .ToListAsync();

        var daysList = new List<CalendarDayResponse>();

        for (int day = 1; day <= daysInMonth; day++)
        {
            var currentDate = new DateOnly(year, month, day);
            var dayTasks = tasks
                .Where(t => t.DueDate.HasValue && DateOnly.FromDateTime(t.DueDate.Value) == currentDate)
                .ToList();

            int completedCount = dayTasks.Count(t => t.IsCompleted);
            int totalCount = dayTasks.Count;
            double rate = totalCount > 0 ? (completedCount / (double)totalCount) * 100.0 : 0.0;

            daysList.Add(new CalendarDayResponse(
                currentDate,
                completedCount,
                totalCount,
                Math.Round(rate, 1),
                dayTasks.Select(TaskResponse.FromEntity).ToList()
            ));
        }

        string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        return new CalendarMonthResponse(year, month, monthName, daysList);
    }
}
