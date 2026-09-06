using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Tasks;

/// <summary>
/// DTO payload for creating a new task.
/// </summary>
public record CreateTaskRequest(
    string Title,
    string Description,
    PriorityLevel Priority,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int? CategoryId,
    RecurrenceType Recurrence = RecurrenceType.None
);
