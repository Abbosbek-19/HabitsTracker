using DailyTaskTracker.Application.DTOs.Calendar;
using DailyTaskTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DailyTaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    private static readonly Guid SeedUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    /// <summary>
    /// Retrieves monthly calendar overview with daily completion statistics.
    /// </summary>
    [HttpGet("{year:int}/{month:int}")]
    [ProducesResponseType(typeof(CalendarMonthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CalendarMonthResponse>> GetMonthlyCalendar(int year, int month)
    {
        if (month < 1 || month > 12)
        {
            return BadRequest(new { status = 400, message = "Month must be between 1 and 12." });
        }

        var result = await _calendarService.GetMonthlyCalendarAsync(year, month, SeedUserId);
        return Ok(result);
    }
}
