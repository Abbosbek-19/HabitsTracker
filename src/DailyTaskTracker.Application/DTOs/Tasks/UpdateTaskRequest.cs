using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Tasks;

/// <summary>
/// DTO payload for updating an existing task.
/// </summary>
public record UpdateTaskRequest(
    string Title,
    string Description,
    PriorityLevel Priority,
    DateTime? DueDate,
    int? EstimatedMinutes,
    int? CategoryId,
    RecurrenceType Recurrence
);
