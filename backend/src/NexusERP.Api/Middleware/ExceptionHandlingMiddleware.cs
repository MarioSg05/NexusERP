using Microsoft.AspNetCore.Mvc;
using NexusERP.Domain.Exceptions;
using FluentValidation;

namespace NexusERP.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException exception)
        {
            await HandleValidationException(context, exception);
        }
        catch (DomainException exception)
        {
            await HandleDomainException(context, exception);
        }
        catch (Exception exception)
        {
            await HandleUnexpectedException(context, exception);
        }
    }

    private async Task HandleValidationException(
    HttpContext context,
    ValidationException exception)
    {
        _logger.LogWarning(exception, exception.Message);

        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray());

        var problem = new ValidationProblemDetails(errors)
        {
            Title = "Validation failed",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsJsonAsync(problem);
    }

    private static async Task WriteProblem(
    HttpContext context,
    int status,
    string title,
    string detail)
    {
        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;

        await context.Response.WriteAsJsonAsync(problem);
    }

    private async Task HandleDomainException(
    HttpContext context,
    DomainException exception)
    {
        _logger.LogWarning(exception, exception.Message);

        await WriteProblem(
            context,
            StatusCodes.Status400BadRequest,
            "Business Rule Violation",
            exception.Message);
    }

    private async Task HandleUnexpectedException(
    HttpContext context,
    Exception exception)
    {
        _logger.LogError(exception, exception.Message);

        await WriteProblem(
            context,
            StatusCodes.Status500InternalServerError,
            "Internal Server Error",
            "An unexpected error occurred.");
    }

    
}