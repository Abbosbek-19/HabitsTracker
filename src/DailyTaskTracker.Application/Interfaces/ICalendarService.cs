using DailyTaskTracker.Application.DTOs.Calendar;

namespace DailyTaskTracker.Application.Interfaces;

public interface ICalendarService
{
    Task<CalendarMonthResponse> GetMonthlyCalendarAsync(int year, int month, Guid userId);
}
