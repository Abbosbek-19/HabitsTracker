using DailyTaskTracker.Application.DTOs.Statistics;
using DailyTaskTracker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DailyTaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    private static readonly Guid SeedUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    /// <summary>
    /// Retrieves overall user task and habit statistics overview.
    /// </summary>
    [HttpGet("overview")]
    [ProducesResponseType(typeof(StatisticsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<StatisticsResponse>> GetOverview()
    {
        var result = await _statisticsService.GetStatisticsOverviewAsync(SeedUserId);
        return Ok(result);
    }
}
