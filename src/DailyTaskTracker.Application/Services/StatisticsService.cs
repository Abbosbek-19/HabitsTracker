using DailyTaskTracker.Application.DTOs.Statistics;
using DailyTaskTracker.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DailyTaskTracker.Application.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IApplicationDbContext _context;

    public StatisticsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsResponse> GetStatisticsOverviewAsync(Guid userId)
    {
        var tasks = await _context.Tasks.AsNoTracking()
            .Where(t => t.UserId == userId || userId == Guid.Empty)
            .ToListAsync();

        int totalTasks = tasks.Count;
        int completedTasks = tasks.Count(t => t.IsCompleted);
        int incompleteTasks = totalTasks - completedTasks;
        double completionRate = totalTasks > 0 ? (completedTasks / (double)totalTasks) * 100.0 : 0.0;

        var habits = await _context.Habits.AsNoTracking()
            .Include(h => h.Completions)
            .Where(h => h.UserId == userId || userId == Guid.Empty)
            .ToListAsync();

        int activeStreaks = habits.Count(h => h.Completions.Any(c => c.CompletedDate == DateOnly.FromDateTime(DateTime.UtcNow)));

        // Weekly breakdown by DayOfWeek
        var dayOfWeekStats = Enum.GetValues<DayOfWeek>()
            .Select(dow =>
            {
                var dayTasks = tasks.Where(t => t.CreatedAtUtc.DayOfWeek == dow).ToList();
                int dayTotal = dayTasks.Count;
                int dayCompleted = dayTasks.Count(t => t.IsCompleted);
                double dayRate = dayTotal > 0 ? (dayCompleted / (double)dayTotal) * 100.0 : 0.0;

                return new WeeklyDayStat(dow.ToString(), dayTotal, dayCompleted, Math.Round(dayRate, 1));
            })
            .ToList();

        var bestDayStat = dayOfWeekStats.OrderByDescending(s => s.CompletionRate).FirstOrDefault();
        var worstDayStat = dayOfWeekStats.Where(s => s.TotalTasks > 0).OrderBy(s => s.CompletionRate).FirstOrDefault();

        return new StatisticsResponse(
            totalTasks,
            completedTasks,
            incompleteTasks,
            Math.Round(completionRate, 1),
            activeStreaks,
            bestDayStat?.DayOfWeek ?? "N/A",
            worstDayStat?.DayOfWeek ?? "N/A",
            dayOfWeekStats
        );
    }
}
