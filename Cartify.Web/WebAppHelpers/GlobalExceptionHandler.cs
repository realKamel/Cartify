using Cartify.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cartify.Web.WebAppHelpers;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var logLevel = exception switch
        {
            ValidationException or NotFoundException or ConflictException => LogLevel.Warning,
            _ => LogLevel.Error
        };
        logger.Log(
            logLevel,
            exception,
            "Unhandled exception occurred for {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path
        );
        var isDevelopment = httpContext.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() ?? false;

        httpContext.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            SecurityTokenException => StatusCodes.Status401Unauthorized,
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };


        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path,
            Detail = isDevelopment ? exception.Message : GetSafeErrorMessage(exception),
            Title = isDevelopment ? exception.GetType().Name : "An error occurred",
            Status = httpContext.Response.StatusCode,
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTime.UtcNow
            }
        };

        if (isDevelopment)
        {
            problemDetails.Extensions["debug"] = new
            {
                exception.StackTrace,
                innerException = exception.InnerException?.Message
            };
        }

        var status = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });

        if (!status)
        {
            logger.LogWarning("Failed to write ProblemDetails for exception {ExceptionType}", exception.GetType().Name);
        }

        return status;
    }

    private static string GetSafeErrorMessage(Exception exception) => exception switch
    {
        ValidationException => "The request contains invalid data.",
        SecurityTokenException => "Authentication failed.",
        NotFoundException => "The requested resource was not found.",
        ConflictException => "A conflict occurred while processing your request.",
        _ => "An unexpected error occurred."
    };
}