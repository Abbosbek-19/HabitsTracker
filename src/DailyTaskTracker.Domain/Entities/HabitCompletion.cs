namespace DailyTaskTracker.Domain.Entities;

/// <summary>
/// Individual date completion record for a habit.
/// </summary>
public class HabitCompletion
{
    public Guid Id { get; private set; }
    public Guid HabitId { get; private set; }
    public Habit? Habit { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public DateOnly CompletedDate { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private HabitCompletion() { }

    public HabitCompletion(Guid habitId, Guid userId, DateOnly completedDate)
    {
        Id = Guid.NewGuid();
        HabitId = habitId;
        UserId = userId;
        CompletedDate = completedDate;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
