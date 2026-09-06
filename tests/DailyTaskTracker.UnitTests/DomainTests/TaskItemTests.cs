using System;
using DailyTaskTracker.Domain.Entities;
using DailyTaskTracker.Domain.Enums;
using Xunit;

namespace DailyTaskTracker.UnitTests.DomainTests;

public class TaskItemTests
{
    private readonly Guid _testUserId = Guid.NewGuid();

    [Fact]
    public void TaskItem_Creation_SetsDefaultPropertiesCorrectly()
    {
        // Arrange
        string title = "Study C# Design Patterns";
        string description = "Learn Singleton, Factory, and Repository patterns";

        // Act
        var task = new TaskItem(title, description, PriorityLevel.High, _testUserId, estimatedMinutes: 45);

        // Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(PriorityLevel.High, task.Priority);
        Assert.False(task.IsCompleted);
        Assert.Equal(_testUserId, task.UserId);
        Assert.Equal(45, task.EstimatedMinutes);
        Assert.True(task.CreatedAtUtc <= DateTime.UtcNow);
    }

    [Fact]
    public void TaskItem_CreationWithEmptyTitle_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new TaskItem("", "Description", PriorityLevel.Low, _testUserId));
    }

    [Fact]
    public void MarkAsCompleted_SetsIsCompletedTrue_AndAddsCompletionRecord()
    {
        // Arrange
        var task = new TaskItem("Test Task", "Desc", PriorityLevel.Medium, _testUserId);

        // Act
        task.MarkAsCompleted();

        // Assert
        Assert.True(task.IsCompleted);
        Assert.NotNull(task.UpdatedAtUtc);
        Assert.Single(task.Completions);
        Assert.Equal(task.Id, task.Completions[0].TaskId);
    }

    [Fact]
    public void MarkAsIncomplete_SetsIsCompletedFalse()
    {
        // Arrange
        var task = new TaskItem("Test Task", "Desc", PriorityLevel.Medium, _testUserId);
        task.MarkAsCompleted();

        // Act
        task.MarkAsIncomplete();

        // Assert
        Assert.False(task.IsCompleted);
    }

    [Fact]
    public void IsOverdue_ReturnsTrue_WhenDueDateInPastAndNotCompleted()
    {
        // Arrange
        var pastDate = DateTime.UtcNow.AddHours(-2);
        var task = new TaskItem("Overdue Task", "Desc", PriorityLevel.Urgent, _testUserId, dueDate: pastDate);

        // Assert
        Assert.True(task.IsOverdue());
    }
}
