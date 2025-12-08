using Cartify.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Cartify.Web.WebAppHelpers;

public class GlobalExceptionHandler(
	ILogger<GlobalExceptionHandler> logger,
	IProblemDetailsService problemDetailsService,
	IHostEnvironment env)
	: IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
		CancellationToken cancellationToken)
	{
		var logLevel = exception switch
		{
			BadRequestException or NotFoundException or ConflictException => LogLevel.Warning,
			_ => LogLevel.Error
		};
		logger.Log(
			logLevel,
			exception,
			"Unhandled exception occurred for {Method} {Path}",
			httpContext.Request.Method,
			httpContext.Request.Path
		);
		var isDevelopment = env.IsDevelopment();

		httpContext.Response.StatusCode = exception switch
		{
			BadRequestException => StatusCodes.Status400BadRequest,
			SecurityTokenException => StatusCodes.Status401Unauthorized,
			UnauthorizedAccessException => StatusCodes.Status403Forbidden,
			NotFoundException => StatusCodes.Status404NotFound,
			ConflictException => StatusCodes.Status409Conflict,
			RateLimitExceededException => StatusCodes.Status429TooManyRequests,
			NotImplementedException => StatusCodes.Status501NotImplemented,
			_ => StatusCodes.Status500InternalServerError
		};


		var problemDetails = new ProblemDetails
		{
			Instance = httpContext.Request.Path,
			//Detail = isDevelopment ? exception.Message : GetSafeErrorMessage(exception),
			Detail = exception is SecurityTokenException ? GetSafeErrorMessage(exception) : isDevelopment ? exception.Message : GetSafeErrorMessage(exception),
			Title = isDevelopment ? exception.GetType().Name : "An error occurred",
			Status = httpContext.Response.StatusCode,
			Extensions =
			{
				["traceId"] = httpContext.TraceIdentifier,
				["timestamp"] = DateTimeOffset.UtcNow
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
			httpContext.Response.ContentType = "application/json";
			await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
		}

		return status;
	}

	private static string GetSafeErrorMessage(Exception exception) => exception switch
	{
		BadRequestException => "The request contains invalid data.",
		SecurityTokenException => "Authentication failed.",
		NotFoundException => "The requested resource was not found.",
		ConflictException => "A error occurred while processing your request.",
		_ => "An unexpected error occurred."
	};
}