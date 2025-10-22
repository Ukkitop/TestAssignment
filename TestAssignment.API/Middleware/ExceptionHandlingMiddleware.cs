using System.Text.Json;
using TestAssignment.Application.Interfaces;
using TestAssignment.Application.Models;
using TestAssignment.Domain.Exceptions;

namespace TestAssignment.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private static long _eventIdCounter = 1;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IJournalService journalService)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception, journalService);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, IJournalService journalService)
    {
        var eventId = Interlocked.Increment(ref _eventIdCounter);

        // Read request body
        string? requestBody = null;
        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body);
            requestBody = await reader.ReadToEndAsync();
        }

        // Log exception to journal
        try
        {
            await journalService.LogExceptionAsync(
                eventId,
                exception,
                context.Request.QueryString.ToString(),
                requestBody,
                context.Request.Path,
                context.Request.Method);
        }
        catch (Exception logException)
        {
            _logger.LogError(logException, "Failed to log exception to database");
        }

        // Log to console
        _logger.LogError(exception, $"Exception occurred. EventId: {eventId}");

        // Prepare response
        var response = new ErrorResponse();
        int statusCode;

        if (exception is SecureException secureException)
        {
            statusCode = StatusCodes.Status500InternalServerError;
            response.Type = "Secure";
            response.Data = new
            {
                message = secureException.Message
            };
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;
            response.Type = "Exception";
            response.Data = new { };
        }

        response.Id = eventId;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}

