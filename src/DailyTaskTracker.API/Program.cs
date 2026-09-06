using DailyTaskTracker.API.Middleware;
using DailyTaskTracker.Application.Interfaces;
using DailyTaskTracker.Application.Services;
using DailyTaskTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure EF Core DbContext with PostgreSQL or InMemory fallback for testing
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase", false);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (!useInMemory && !string.IsNullOrEmpty(connectionString) && !connectionString.Contains("Memory"))
    {
        options.UseNpgsql(connectionString, b => b.MigrationsAssembly("DailyTaskTracker.Infrastructure"));
    }
    else
    {
        options.UseInMemoryDatabase("DailyTaskTrackerDb");
    }
});

// Register Application Interfaces and Scoped Services (DI Scoped lifetime)
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

// Configure OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Daily Task & Habit Tracker API",
        Version = "v1",
        Description = "ASP.NET Core REST API backed by EF Core and PostgreSQL."
    });
});

// Configure CORS for React frontend connection
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Global Exception Handling Middleware (MUST be first in HTTP pipeline)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Ensure Database Created / Auto-seed baseline data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    SeedDatabaseData(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Daily Task Tracker API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDatabaseData(ApplicationDbContext context)
{
    if (!context.Tasks.Any())
    {
        var seedUser = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var task1 = new DailyTaskTracker.Domain.Entities.TaskItem(
            "Study EF Core & PostgreSQL",
            "Master DbContext, Migrations, Fluent API, and LINQ async queries",
            DailyTaskTracker.Domain.Enums.PriorityLevel.Urgent,
            seedUser,
            DateTime.UtcNow.AddHours(4),
            120
        );

        var task2 = new DailyTaskTracker.Domain.Entities.TaskItem(
            "Build React Frontend Dashboard",
            "Connect React + TypeScript client to ASP.NET Core REST API",
            DailyTaskTracker.Domain.Enums.PriorityLevel.High,
            seedUser,
            DateTime.UtcNow.AddDays(1),
            90
        );

        task1.MarkAsCompleted();

        context.Tasks.AddRange(task1, task2);
    }

    if (!context.Habits.Any())
    {
        var seedUser = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var habit1 = new DailyTaskTracker.Domain.Entities.Habit(
            "Drink 2L Water",
            "Daily hydration tracking",
            DailyTaskTracker.Domain.Enums.HabitFrequency.Daily,
            1,
            seedUser
        );

        var habit2 = new DailyTaskTracker.Domain.Entities.Habit(
            "Study C#",
            "Practice coding daily",
            DailyTaskTracker.Domain.Enums.HabitFrequency.Daily,
            1,
            seedUser
        );

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        habit1.RecordCompletion(today);
        habit1.RecordCompletion(today.AddDays(-1));
        habit1.RecordCompletion(today.AddDays(-2));

        context.Habits.AddRange(habit1, habit2);
    }

    context.SaveChanges();
}
