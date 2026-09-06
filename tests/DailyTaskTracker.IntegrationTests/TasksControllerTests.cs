using System;
using System.Threading.Tasks;
using DailyTaskTracker.API.Controllers;
using DailyTaskTracker.Application.DTOs.Common;
using DailyTaskTracker.Application.DTOs.Tasks;
using DailyTaskTracker.Application.Services;
using DailyTaskTracker.Domain.Enums;
using DailyTaskTracker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DailyTaskTracker.IntegrationTests;

public class TasksControllerTests
{
    private readonly TasksController _controller;
    private readonly ApplicationDbContext _dbContext;

    public TasksControllerTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        var taskService = new TaskService(_dbContext);
        _controller = new TasksController(taskService);
    }

    [Fact]
    public async Task Create_And_GetPaged_ReturnsPagedResponseFromEFCore()
    {
        // Arrange
        var createRequest = new CreateTaskRequest(
            "Test EF Core Persistence Task",
            "Validating Database Insertion",
            PriorityLevel.High,
            DateTime.UtcNow.AddDays(1),
            45,
            null
        );

        // Act
        var createResponse = await _controller.Create(createRequest);
        var createdResult = Assert.IsType<CreatedAtActionResult>(createResponse.Result);
        var createdTask = Assert.IsType<TaskResponse>(createdResult.Value);

        var filter = new TaskQueryFilter();
        var getPagedResponse = await _controller.GetPaged(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(getPagedResponse.Result);
        var pagedData = Assert.IsType<PagedResponse<TaskResponse>>(okResult.Value);
        Assert.Contains(pagedData.Items, t => t.Id == createdTask.Id);
    }

    [Fact]
    public async Task Delete_ExistingTask_RemovesFromDatabase()
    {
        // Arrange
        var createRequest = new CreateTaskRequest("Task To Delete", "Desc", PriorityLevel.Low, null, null, null);
        var createResult = await _controller.Create(createRequest);
        var createdTask = (TaskResponse)((CreatedAtActionResult)createResult.Result!).Value!;

        // Act
        var deleteResult = await _controller.Delete(createdTask.Id);

        // Assert
        Assert.IsType<NoContentResult>(deleteResult);
        var getByIdResult = await _controller.GetById(createdTask.Id);
        Assert.IsType<NotFoundObjectResult>(getByIdResult.Result);
    }
}
