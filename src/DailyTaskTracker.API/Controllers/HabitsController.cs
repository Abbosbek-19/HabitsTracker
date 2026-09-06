using DailyTaskTracker.Application.DTOs.Habits;
using DailyTaskTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DailyTaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HabitsController : ControllerBase
{
    private readonly IHabitService _habitService;
    private static readonly Guid SeedUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public HabitsController(IHabitService habitService)
    {
        _habitService = habitService;
    }

    /// <summary>
    /// Retrieves all user habits with streak stats and completion history.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HabitResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<HabitResponse>>> GetAll()
    {
        var habits = await _habitService.GetAllHabitsAsync(SeedUserId);
        return Ok(habits);
    }

    /// <summary>
    /// Retrieves a specific habit by its unique ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HabitResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HabitResponse>> GetById(Guid id)
    {
        var habit = await _habitService.GetHabitByIdAsync(id, SeedUserId);
        if (habit == null)
        {
            return NotFound(new { status = 404, message = $"Habit with ID '{id}' was not found." });
        }

        return Ok(habit);
    }

    /// <summary>
    /// Creates a new habit tracker.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(HabitResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HabitResponse>> Create([FromBody] CreateHabitRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { status = 400, message = "Habit name is required." });
        }

        var createdHabit = await _habitService.CreateHabitAsync(request, SeedUserId);
        return CreatedAtAction(nameof(GetById), new { id = createdHabit.Id }, createdHabit);
    }

    /// <summary>
    /// Updates an existing habit.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(HabitResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<HabitResponse>> Update(Guid id, [FromBody] UpdateHabitRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { status = 400, message = "Habit name is required." });
        }

        var updatedHabit = await _habitService.UpdateHabitAsync(id, request, SeedUserId);
        if (updatedHabit == null)
        {
            return NotFound(new { status = 404, message = $"Habit with ID '{id}' was not found." });
        }

        return Ok(updatedHabit);
    }

    /// <summary>
    /// Deletes a habit by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _habitService.DeleteHabitAsync(id, SeedUserId);
        if (!success)
        {
            return NotFound(new { status = 404, message = $"Habit with ID '{id}' was not found." });
        }

        return NoContent();
    }

    /// <summary>
    /// Records a habit completion for today or a specified target date.
    /// </summary>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(HabitResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HabitResponse>> RecordCompletion(Guid id, [FromQuery] DateOnly? date)
    {
        var habit = await _habitService.RecordCompletionAsync(id, date, SeedUserId);
        if (habit == null)
        {
            return NotFound(new { status = 404, message = $"Habit with ID '{id}' was not found." });
        }

        return Ok(habit);
    }
}
