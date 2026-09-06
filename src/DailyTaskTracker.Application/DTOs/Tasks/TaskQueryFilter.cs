using DailyTaskTracker.Domain.Enums;

namespace DailyTaskTracker.Application.DTOs.Tasks;

/// <summary>
/// Query parameter filter payload for searching, filtering, sorting, and paginating tasks.
/// </summary>
public record TaskQueryFilter(
    PriorityLevel? Priority = null,
    int? CategoryId = null,
    bool? IsCompleted = null,
    string? Search = null,
    string? SortBy = "DueDate", // DueDate, Priority, CreatedAt, Title
    bool SortDescending = false,
    int PageNumber = 1,
    int PageSize = 10
);
