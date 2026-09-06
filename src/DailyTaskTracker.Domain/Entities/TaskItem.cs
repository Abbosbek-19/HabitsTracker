using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Domain.Entities;

/// <summary>
/// Represents a daily task item created by a user.
/// </summary>
public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public PriorityLevel Priority { get; private set; }
    public int? CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public DateTime? DueDate { get; private set; }
    public int? EstimatedMinutes { get; private set; }
    public bool IsCompleted { get; private set; }
    public RecurrenceType Recurrence { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
    public Guid UserId { get; private set; }
    public User? User { get; private set; }

    // Navigation property for historical completion tracking
    public List<TaskCompletion> Completions { get; private set; } = new();

    // Required by Entity Framework Core
    private TaskItem() { }

    /// <summary>
    /// Constructs a new domain TaskItem.
    /// </summary>
    public TaskItem(
        string title, 
        string description, 
        PriorityLevel priority, 
        Guid userId, 
        DateTime? dueDate = null, 
        int? estimatedMinutes = null, 
        int? categoryId = null,
        RecurrenceType recurrence = RecurrenceType.None)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Task title cannot be empty or whitespace.", nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        Priority = priority;
        UserId = userId;
        DueDate = dueDate;
        EstimatedMinutes = estimatedMinutes;
        CategoryId = categoryId;
        Recurrence = recurrence;
        IsCompleted = false;
        CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the task as completed.
    /// </summary>
    public void MarkAsCompleted()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            UpdatedAtUtc = DateTime.UtcNow;
            Completions.Add(new TaskCompletion(Id, UserId, UpdatedAtUtc.Value));
        }
    }

    /// <summary>
    /// Marks the task as incomplete.
    /// </summary>
    public void MarkAsIncomplete()
    {
        if (IsCompleted)
        {
            IsCompleted = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Updates task properties cleanly with encapsulation validation.
    /// </summary>
    public void UpdateDetails(
        string title, 
        string description, 
        PriorityLevel priority, 
        DateTime? dueDate, 
        int? estimatedMinutes, 
        int? categoryId,
        RecurrenceType recurrence)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Task title cannot be empty or whitespace.", nameof(title));
        }

        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        Priority = priority;
        DueDate = dueDate;
        EstimatedMinutes = estimatedMinutes;
        CategoryId = categoryId;
        Recurrence = recurrence;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Evaluates if the task is overdue based on current UTC time.
    /// </summary>
    public bool IsOverdue() => DueDate.HasValue && !IsCompleted && DueDate.Value < DateTime.UtcNow;
}
