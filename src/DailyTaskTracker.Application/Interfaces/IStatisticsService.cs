using DailyTaskTracker.Application.DTOs.Statistics;

namespace DailyTaskTracker.Application.Interfaces;

public interface IStatisticsService
{
    Task<StatisticsResponse> GetStatisticsOverviewAsync(Guid userId);
}
