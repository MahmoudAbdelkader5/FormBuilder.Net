using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FormBuilder.API.ExceptionHandlers;

/// <summary>
/// Handles global exceptions and writes problem details responses.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    /// <summary>
    /// Attempts to handle an exception and write a problem details response.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the exception was handled; otherwise, false.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        var statusCode = DetermineStatusCode(exception);
        var title = GetExceptionTitle(exception);
        var detail = GetExceptionDetail(exception, httpContext);

        // Log the exception
        LogException(exception, httpContext, statusCode);

        ProblemDetails problemDetails = new()
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        // Add additional details for development environment
        if (httpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            problemDetails.Extensions.TryAdd("StackTrace", exception.StackTrace);
            problemDetails.Extensions.TryAdd("ExceptionType", exception.GetType().FullName);
        }

        if (exception.InnerException is not null)
        {
            problemDetails.Extensions.TryAdd(
                "InnerExceptionType",
                exception.InnerException.GetType().Name
            );
            problemDetails.Extensions.TryAdd("InnerExceptionMessage", exception.InnerException.Message);
        }

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(
            new()
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails,
            }
        );
    }

    private int DetermineStatusCode(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException or ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status400BadRequest, // Changed from 404 to 400 for duplicate validation
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            DbUpdateException => StatusCodes.Status400BadRequest,
            TimeoutException => StatusCodes.Status408RequestTimeout,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private string GetExceptionTitle(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => "Invalid Request",
            ArgumentException => "Invalid Argument",
            UnauthorizedAccessException => "Unauthorized",
            KeyNotFoundException => "Resource Not Found",
            InvalidOperationException => "Invalid Operation",
            DbUpdateConcurrencyException => "Concurrency Conflict",
            DbUpdateException => "Database Error",
            TimeoutException => "Request Timeout",
            _ => "Internal Server Error"
        };
    }

    private string GetExceptionDetail(Exception exception, HttpContext httpContext)
    {
        // Don't expose sensitive information in production
        var isDevelopment = httpContext.RequestServices
            .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        if (isDevelopment)
        {
            return exception.Message;
        }

        // Return generic messages in production
        // Note: For InvalidOperationException (like duplicate validation), we return the actual message
        // because it contains important validation information
        return exception switch
        {
            ArgumentNullException or ArgumentException => "The request contains invalid data.",
            UnauthorizedAccessException => "You are not authorized to perform this action.",
            KeyNotFoundException => "The requested resource was not found.",
            InvalidOperationException => exception.Message, // Return actual message for duplicate validation
            DbUpdateConcurrencyException => "The resource has been modified by another user.",
            DbUpdateException => "An error occurred while saving data.",
            TimeoutException => "The request timed out. Please try again.",
            _ => "An error occurred while processing your request."
        };
    }

    private void LogException(Exception exception, HttpContext httpContext, int statusCode)
    {
        var logLevel = statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;

        logger.Log(
            logLevel,
            exception,
            "Exception occurred: {ExceptionType} - {Message} | Path: {Path} | Method: {Method} | StatusCode: {StatusCode}",
            exception.GetType().Name,
            exception.Message,
            httpContext.Request.Path,
            httpContext.Request.Method,
            statusCode
        );
    }
}
