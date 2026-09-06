using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Tasks;

/// <summary>
/// API Response DTO for exposing Task data safely without leaking domain models.
/// </summary>
public record TaskResponse(
    Guid Id,
    string Title,
    string Description,
    PriorityLevel Priority,
    string PriorityName,
    int? CategoryId,
    DateTime? DueDate,
    int? EstimatedMinutes,
    bool IsCompleted,
    bool IsOverdue,
    RecurrenceType Recurrence,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
)
{
    /// <summary>
    /// Factory mapping method converting a Domain TaskItem entity to a TaskResponse DTO.
    /// </summary>
    public static TaskResponse FromEntity(TaskItem entity)
    {
        return new TaskResponse(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.Priority,
            entity.Priority.ToString(),
            entity.CategoryId,
            entity.DueDate,
            entity.EstimatedMinutes,
            entity.IsCompleted,
            entity.IsOverdue(),
            entity.Recurrence,
            entity.CreatedAtUtc,
            entity.UpdatedAtUtc
        );
    }
}
