using DailyTaskTracker.Application.DTOs.Habits;
using DailyTaskTracker.Application.Interfaces;
using DailyTaskTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DailyTaskTracker.Application.Services;

/// <summary>
/// Habit Service executing EF Core persistence, streak calculation algorithms, and habit completion tracking.
/// </summary>
public class HabitService : IHabitService
{
    private readonly IApplicationDbContext _context;

    public HabitService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HabitResponse>> GetAllHabitsAsync(Guid userId)
    {
        var habits = await _context.Habits.AsNoTracking()
            .Include(h => h.Completions)
            .Where(h => h.UserId == userId || userId == Guid.Empty)
            .OrderByDescending(h => h.CreatedAtUtc)
            .ToListAsync();

        return habits.Select(MapToResponse);
    }

    public async Task<HabitResponse?> GetHabitByIdAsync(Guid id, Guid userId)
    {
        var habit = await _context.Habits.AsNoTracking()
            .Include(h => h.Completions)
            .FirstOrDefaultAsync(h => h.Id == id && (h.UserId == userId || userId == Guid.Empty));

        return habit == null ? null : MapToResponse(habit);
    }

    public async Task<HabitResponse> CreateHabitAsync(CreateHabitRequest request, Guid userId)
    {
        var effectiveUserId = userId == Guid.Empty ? Guid.Parse("11111111-1111-1111-1111-111111111111") : userId;

        var habit = new Habit(
            request.Name,
            request.Description,
            request.Frequency,
            request.TargetPerPeriod,
            effectiveUserId
        );

        _context.Habits.Add(habit);
        await _context.SaveChangesAsync();

        return MapToResponse(habit);
    }

    public async Task<HabitResponse?> UpdateHabitAsync(Guid id, UpdateHabitRequest request, Guid userId)
    {
        var habit = await _context.Habits
            .Include(h => h.Completions)
            .FirstOrDefaultAsync(h => h.Id == id && (h.UserId == userId || userId == Guid.Empty));

        if (habit == null) return null;

        habit.UpdateDetails(request.Name, request.Description, request.Frequency, request.TargetPerPeriod);
        await _context.SaveChangesAsync();

        return MapToResponse(habit);
    }

    public async Task<bool> DeleteHabitAsync(Guid id, Guid userId)
    {
        var habit = await _context.Habits
            .FirstOrDefaultAsync(h => h.Id == id && (h.UserId == userId || userId == Guid.Empty));

        if (habit == null) return false;

        _context.Habits.Remove(habit);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<HabitResponse?> RecordCompletionAsync(Guid habitId, DateOnly? date, Guid userId)
    {
        var habit = await _context.Habits
            .Include(h => h.Completions)
            .FirstOrDefaultAsync(h => h.Id == habitId && (h.UserId == userId || userId == Guid.Empty));

        if (habit == null) return null;

        DateOnly targetDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        // Check if already completed for target date
        if (!habit.Completions.Any(c => c.CompletedDate == targetDate))
        {
            var completion = habit.RecordCompletion(targetDate);
            _context.HabitCompletions.Add(completion);
            await _context.SaveChangesAsync();
        }

        return MapToResponse(habit);
    }

    /// <summary>
    /// Converts a Domain Habit entity into a HabitResponse DTO, executing streak calculation algorithms.
    /// </summary>
    private static HabitResponse MapToResponse(Habit habit)
    {
        var sortedDates = habit.Completions
            .Select(c => c.CompletedDate)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        int currentStreak = CalculateCurrentStreak(sortedDates);
        int longestStreak = CalculateLongestStreak(sortedDates);

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        bool isCompletedToday = sortedDates.Contains(today);

        double totalDaysActive = Math.Max(1, (today.ToDateTime(TimeOnly.MinValue) - habit.CreatedAtUtc.Date).TotalDays + 1);
        double completionRate = Math.Min(100.0, (sortedDates.Count / totalDaysActive) * 100.0);

        return new HabitResponse(
            habit.Id,
            habit.Name,
            habit.Description,
            habit.Frequency,
            habit.TargetPerPeriod,
            currentStreak,
            longestStreak,
            Math.Round(completionRate, 1),
            isCompletedToday,
            habit.CreatedAtUtc,
            sortedDates
        );
    }

    private static int CalculateCurrentStreak(List<DateOnly> sortedDates)
    {
        if (!sortedDates.Any()) return 0;

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly yesterday = today.AddDays(-1);

        if (sortedDates[0] != today && sortedDates[0] != yesterday)
        {
            return 0;
        }

        int streak = 1;
        for (int i = 0; i < sortedDates.Count - 1; i++)
        {
            if (sortedDates[i].AddDays(-1) == sortedDates[i + 1])
            {
                streak++;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    private static int CalculateLongestStreak(List<DateOnly> sortedDates)
    {
        if (!sortedDates.Any()) return 0;

        var datesAscending = sortedDates.OrderBy(d => d).ToList();
        int maxStreak = 1;
        int currentStreak = 1;

        for (int i = 0; i < datesAscending.Count - 1; i++)
        {
            if (datesAscending[i].AddDays(1) == datesAscending[i + 1])
            {
                currentStreak++;
                maxStreak = Math.Max(maxStreak, currentStreak);
            }
            else if (datesAscending[i] != datesAscending[i + 1])
            {
                currentStreak = 1;
            }
        }

        return maxStreak;
    }
}
