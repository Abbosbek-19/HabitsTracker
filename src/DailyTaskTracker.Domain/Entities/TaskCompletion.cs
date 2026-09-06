namespace DailyTaskTracker.Domain.Entities;

/// <summary>
/// Historical record of a task completion instance (for streak and analytics tracking).
/// </summary>
public class TaskCompletion
{
    public Guid Id { get; private set; }
    public Guid TaskId { get; private set; }
    public TaskItem? Task { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }
    public DateTime CompletedAtUtc { get; private set; }

    private TaskCompletion() { }

    public TaskCompletion(Guid taskId, Guid userId, DateTime completedAtUtc)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        UserId = userId;
        CompletedAtUtc = completedAtUtc;
    }
}
