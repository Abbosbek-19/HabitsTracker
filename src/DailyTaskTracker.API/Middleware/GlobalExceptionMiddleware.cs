using System.Net;
using System.Text.Json;

namespace DailyTaskTracker.API.Middleware;

/// <summary>
/// Centralized ASP.NET Core Middleware catching unhandled exceptions and returning standard JSON error responses.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception intercepted by GlobalExceptionMiddleware: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ArgumentException argEx => ((int)HttpStatusCode.BadRequest, argEx.Message),
            KeyNotFoundException notFoundEx => ((int)HttpStatusCode.NotFound, notFoundEx.Message),
            UnauthorizedAccessException authEx => ((int)HttpStatusCode.Unauthorized, authEx.Message),
            _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred. Please try again later.")
        };

        context.Response.StatusCode = statusCode;

        var responsePayload = new
        {
            status = statusCode,
            message,
            timestamp = DateTime.UtcNow
        };

        var jsonResponse = JsonSerializer.Serialize(responsePayload);
        return context.Response.WriteAsync(jsonResponse);
    }
}
