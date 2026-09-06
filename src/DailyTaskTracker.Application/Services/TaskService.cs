using DailyTaskTracker.Application.DTOs.Common;
using DailyTaskTracker.Application.DTOs.Tasks;
using DailyTaskTracker.Application.Interfaces;
using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;
using DailyTaskTracker.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace DailyTaskTracker.Application.Services;

/// <summary>
/// Task Service handling EF Core querying, pagination, search, sorting, and recurrence processing.
/// </summary>
public class TaskService : ITaskService
{
    private readonly IApplicationDbContext _context;

    public TaskService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<TaskResponse>> GetPagedTasksAsync(Guid userId, TaskQueryFilter filter)
    {
        var query = _context.Tasks.AsNoTracking()
            .Where(t => t.UserId == userId || userId == Guid.Empty);

        // Filtering
        if (filter.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == filter.Priority.Value);
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
        }

        if (filter.IsCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == filter.IsCompleted.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            string searchLower = filter.Search.Trim().ToLower();
            query = query.Where(t => 
                t.Title.ToLower().Contains(searchLower) ||
                t.Description.ToLower().Contains(searchLower));
        }

        // Sorting
        query = (filter.SortBy?.ToLower(), filter.SortDescending) switch
        {
            ("priority", true) => query.OrderByDescending(t => t.Priority),
            ("priority", false) => query.OrderBy(t => t.Priority),
            ("title", true) => query.OrderByDescending(t => t.Title),
            ("title", false) => query.OrderBy(t => t.Title),
            ("createdat", true) => query.OrderByDescending(t => t.CreatedAtUtc),
            ("createdat", false) => query.OrderBy(t => t.CreatedAtUtc),
            (_, true) => query.OrderByDescending(t => t.DueDate),
            _ => query.OrderBy(t => t.DueDate)
        };

        int totalCount = await query.CountAsync();

        var tasks = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var responses = tasks.Select(TaskResponse.FromEntity);

        return PagedResponse<TaskResponse>.Create(responses, totalCount, filter.PageNumber, filter.PageSize);
    }

    public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync(Guid userId, PriorityLevel? priority = null, bool? isCompleted = null, string? search = null)
    {
        var filter = new TaskQueryFilter(priority, null, isCompleted, search, "DueDate", false, 1, 1000);
        var pagedResult = await GetPagedTasksAsync(userId, filter);
        return pagedResult.Items;
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || userId == Guid.Empty));

        return task == null ? null : TaskResponse.FromEntity(task);
    }

    public async Task<TaskResponse> CreateTaskAsync(CreateTaskRequest request, Guid userId)
    {
        var effectiveUserId = userId == Guid.Empty ? Guid.Parse("11111111-1111-1111-1111-111111111111") : userId;

        var task = new TaskItem(
            request.Title,
            request.Description,
            request.Priority,
            effectiveUserId,
            request.DueDate,
            request.EstimatedMinutes,
            request.CategoryId,
            request.Recurrence
        );

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return TaskResponse.FromEntity(task);
    }

    public async Task<TaskResponse?> UpdateTaskAsync(Guid id, UpdateTaskRequest request, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || userId == Guid.Empty));

        if (task == null) return null;

        task.UpdateDetails(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate,
            request.EstimatedMinutes,
            request.CategoryId,
            request.Recurrence
        );

        await _context.SaveChangesAsync();

        return TaskResponse.FromEntity(task);
    }

    public async Task<bool> DeleteTaskAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || userId == Guid.Empty));

        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<TaskResponse?> MarkTaskAsCompletedAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .Include(t => t.Completions)
            .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || userId == Guid.Empty));

        if (task == null) return null;

        task.MarkAsCompleted();

        // Process Recurrence Engine: Auto-spawn next recurring task instance
        if (task.Recurrence != RecurrenceType.None)
        {
            var nextTaskInstance = RecurrenceEngine.GenerateNextRecurrenceInstance(task);
            if (nextTaskInstance != null)
            {
                _context.Tasks.Add(nextTaskInstance);
            }
        }

        await _context.SaveChangesAsync();

        return TaskResponse.FromEntity(task);
    }

    public async Task<TaskResponse?> MarkTaskAsIncompleteAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || userId == Guid.Empty));

        if (task == null) return null;

        task.MarkAsIncomplete();
        await _context.SaveChangesAsync();

        return TaskResponse.FromEntity(task);
    }
}
