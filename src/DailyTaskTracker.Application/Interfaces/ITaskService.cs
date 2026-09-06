using DailyTaskTracker.Application.DTOs.Common;
using DailyTaskTracker.Application.DTOs.Tasks;

namespace DailyTaskTracker.Application.Interfaces;

/// <summary>
/// Service interface defining application logic operations for tasks.
/// </summary>
public interface ITaskService
{
    Task<PagedResponse<TaskResponse>> GetPagedTasksAsync(Guid userId, TaskQueryFilter filter);
    Task<IEnumerable<TaskResponse>> GetAllTasksAsync(Guid userId, Domain.Enums.PriorityLevel? priority = null, bool? isCompleted = null, string? search = null);
    Task<TaskResponse?> GetTaskByIdAsync(Guid id, Guid userId);
    Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, Guid userId);
    Task<TaskResponse?> UpdateTaskAsync(Guid id, UpdateTaskRequest request, Guid userId);
    Task<bool> DeleteTaskAsync(Guid id, Guid userId);
    Task<TaskResponse?> MarkTaskAsCompletedAsync(Guid id, Guid userId);
    Task<TaskResponse?> MarkTaskAsIncompleteAsync(Guid id, Guid userId);
}
