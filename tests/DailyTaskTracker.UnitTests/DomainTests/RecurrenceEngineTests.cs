using System;
using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;
using DailyTaskTracker.Domain.Services;
using Xunit;

namespace DailyTaskTracker.UnitTests.DomainTests;

public class RecurrenceEngineTests
{
    private readonly Guid _testUserId = Guid.NewGuid();

    [Fact]
    public void GenerateNextRecurrenceInstance_DailyTask_ReturnsTaskWithNextDayDueDate()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var dailyTask = new TaskItem(
            "Daily C# Coding",
            "Code every day",
            PriorityLevel.High,
            _testUserId,
            dueDate: today,
            recurrence: RecurrenceType.Daily
        );

        // Act
        var nextTask = RecurrenceEngine.GenerateNextRecurrenceInstance(dailyTask);

        // Assert
        Assert.NotNull(nextTask);
        Assert.NotEqual(dailyTask.Id, nextTask.Id); // New GUID
        Assert.Equal(dailyTask.Title, nextTask.Title);
        Assert.Equal(today.AddDays(1), nextTask.DueDate);
        Assert.False(nextTask.IsCompleted);
    }

    [Fact]
    public void GenerateNextRecurrenceInstance_NoneRecurrence_ReturnsNull()
    {
        // Arrange
        var task = new TaskItem("One Time Task", "Desc", PriorityLevel.Low, _testUserId, recurrence: RecurrenceType.None);

        // Act
        var nextTask = RecurrenceEngine.GenerateNextRecurrenceInstance(task);

        // Assert
        Assert.Null(nextTask);
    }
}
