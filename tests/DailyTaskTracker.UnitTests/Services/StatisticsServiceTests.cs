using System;
using System.Threading.Tasks;
using DailyTaskTracker.Application.Services;
using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;
using DailyTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DailyTaskTracker.UnitTests.Services;

public class StatisticsServiceTests
{
    private readonly StatisticsService _statisticsService;
    private readonly ApplicationDbContext _context;

    public StatisticsServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _statisticsService = new StatisticsService(_context);
    }

    [Fact]
    public async Task GetStatisticsOverview_ReturnsAccurateCompletionPercentage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task1 = new TaskItem("Task 1", "Desc", PriorityLevel.High, userId);
        var task2 = new TaskItem("Task 2", "Desc", PriorityLevel.Low, userId);
        task1.MarkAsCompleted();

        _context.Tasks.AddRange(task1, task2);
        await _context.SaveChangesAsync();

        // Act
        var stats = await _statisticsService.GetStatisticsOverviewAsync(userId);

        // Assert
        Assert.Equal(2, stats.TotalTasks);
        Assert.Equal(1, stats.CompletedTasks);
        Assert.Equal(1, stats.IncompleteTasks);
        Assert.Equal(50.0, stats.CompletionRate);
    }
}
