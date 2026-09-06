using DailyTaskTracker.Application.DTOs.Common;
using DailyTaskTracker.Application.DTOs.Tasks;
using DailyTaskTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DailyTaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private static readonly Guid SeedUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Retrieves a paginated, searchable, filterable, and sortable list of tasks.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<TaskResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<TaskResponse>>> GetPaged([FromQuery] TaskQueryFilter filter)
    {
        var result = await _taskService.GetPagedTasksAsync(SeedUserId, filter);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific task by its unique ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> GetById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id, SeedUserId);
        if (task == null)
        {
            return NotFound(new { status = 404, message = $"Task with ID '{id}' was not found." });
        }

        return Ok(task);
    }

    /// <summary>
    /// Creates a new task item.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskResponse>> Create([FromBody] CreateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { status = 400, message = "Task title is required." });
        }

        var createdTask = await _taskService.CreateTaskAsync(request, SeedUserId);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    /// <summary>
    /// Updates an existing task by ID.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskResponse>> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { status = 400, message = "Task title is required." });
        }

        var updatedTask = await _taskService.UpdateTaskAsync(id, request, SeedUserId);
        if (updatedTask == null)
        {
            return NotFound(new { status = 404, message = $"Task with ID '{id}' was not found." });
        }

        return Ok(updatedTask);
    }

    /// <summary>
    /// Deletes a task by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _taskService.DeleteTaskAsync(id, SeedUserId);
        if (!success)
        {
            return NotFound(new { status = 404, message = $"Task with ID '{id}' was not found." });
        }

        return NoContent();
    }

    /// <summary>
    /// Marks a task as completed (Auto-spawns next instance if task is recurring).
    /// </summary>
    [HttpPatch("{id:guid}/complete")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> MarkAsCompleted(Guid id)
    {
        var task = await _taskService.MarkTaskAsCompletedAsync(id, SeedUserId);
        if (task == null)
        {
            return NotFound(new { status = 404, message = $"Task with ID '{id}' was not found." });
        }

        return Ok(task);
    }

    /// <summary>
    /// Marks a task as incomplete.
    /// </summary>
    [HttpPatch("{id:guid}/uncomplete")]
    [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskResponse>> MarkAsIncomplete(Guid id)
    {
        var task = await _taskService.MarkTaskAsIncompleteAsync(id, SeedUserId);
        if (task == null)
        {
            return NotFound(new { status = 404, message = $"Task with ID '{id}' was not found." });
        }

        return Ok(task);
    }
}
