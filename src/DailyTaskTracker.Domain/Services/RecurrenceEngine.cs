using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Domain.Services;

/// <summary>
/// Domain service for calculating next recurrence instances of recurring tasks.
/// </summary>
public static class RecurrenceEngine
{
    /// <summary>
    /// Calculates the next due date for a task based on its RecurrenceType.
    /// </summary>
    public static DateTime CalculateNextDueDate(DateTime baseDate, RecurrenceType recurrence)
    {
        return recurrence switch
        {
            RecurrenceType.Daily => baseDate.AddDays(1),
            RecurrenceType.Weekly => baseDate.AddDays(7),
            RecurrenceType.Monthly => baseDate.AddMonths(1),
            RecurrenceType.SpecificWeekdays => baseDate.AddDays(1), // Default next day increment
            _ => baseDate
        };
    }

    /// <summary>
    /// Spawns a new TaskItem instance for the next recurrence cycle when a recurring task is completed.
    /// </summary>
    public static TaskItem? GenerateNextRecurrenceInstance(TaskItem completedTask)
    {
        if (completedTask.Recurrence == RecurrenceType.None)
        {
            return null;
        }

        DateTime baseDate = completedTask.DueDate ?? DateTime.UtcNow;
        DateTime nextDueDate = CalculateNextDueDate(baseDate, completedTask.Recurrence);

        return new TaskItem(
            completedTask.Title,
            completedTask.Description,
            completedTask.Priority,
            completedTask.UserId,
            nextDueDate,
            completedTask.EstimatedMinutes,
            completedTask.CategoryId,
            completedTask.Recurrence
        );
    }
}
