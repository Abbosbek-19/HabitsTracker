using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Domain.Entities;

/// <summary>
/// Represents a user habit to track daily/weekly consistency.
/// </summary>
public class Habit
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public HabitFrequency Frequency { get; private set; }
    public int TargetPerPeriod { get; private set; } = 1;
    public DateTime CreatedAtUtc { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    public List<HabitCompletion> Completions { get; private set; } = new();

    private Habit() { }

    public Habit(string name, string description, HabitFrequency frequency, int targetPerPeriod, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Habit name cannot be empty.", nameof(name));
        if (targetPerPeriod < 1)
            throw new ArgumentOutOfRangeException(nameof(targetPerPeriod), "Target must be at least 1.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Frequency = frequency;
        TargetPerPeriod = targetPerPeriod;
        CreatedAtUtc = DateTime.UtcNow;
        UserId = userId;
    }

    public void UpdateDetails(string name, string description, HabitFrequency frequency, int targetPerPeriod)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Habit name cannot be empty.", nameof(name));
        if (targetPerPeriod < 1)
            throw new ArgumentOutOfRangeException(nameof(targetPerPeriod), "Target must be at least 1.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Frequency = frequency;
        TargetPerPeriod = targetPerPeriod;
    }

    /// <summary>
    /// Records a habit completion for a target date.
    /// </summary>
    public HabitCompletion RecordCompletion(DateOnly date)
    {
        var completion = new HabitCompletion(Id, UserId, date);
        Completions.Add(completion);
        return completion;
    }
}
