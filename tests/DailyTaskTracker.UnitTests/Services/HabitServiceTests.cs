using System;
using System.Threading.Tasks;
using DailyTaskTracker.Application.DTOs.Habits;
using DailyTaskTracker.Application.Services;
using DailyTaskTracker.Domain.Enums;
using DailyTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DailyTaskTracker.UnitTests.Services;

public class HabitServiceTests
{
    private readonly HabitService _habitService;
    private readonly ApplicationDbContext _context;

    public HabitServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _habitService = new HabitService(_context);
    }

    [Fact]
    public async Task CreateHabit_And_RecordCompletion_CalculatesCurrentStreakCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateHabitRequest("Gym Workout", "Exercise daily", HabitFrequency.Daily, 1);

        // Act
        var habitResponse = await _habitService.CreateHabitAsync(request, userId);
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly yesterday = today.AddDays(-1);

        await _habitService.RecordCompletionAsync(habitResponse.Id, yesterday, userId);
        var updatedResponse = await _habitService.RecordCompletionAsync(habitResponse.Id, today, userId);

        // Assert
        Assert.NotNull(updatedResponse);
        Assert.Equal(2, updatedResponse.CurrentStreak);
        Assert.True(updatedResponse.IsCompletedToday);
    }
}
