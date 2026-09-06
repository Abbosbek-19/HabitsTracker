namespace DailyTaskTracker.Domain.Enums;

/// <summary>
/// Specifies how frequently a task repeats.
/// </summary>
public enum RecurrenceType
{
    None = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3,
    SpecificWeekdays = 4
}
