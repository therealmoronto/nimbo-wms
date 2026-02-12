using System.Net.Mime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Common;

namespace Nimbo.Wms.Http;

[PublicAPI]
public sealed class ProblemDetailsExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProblemDetailsExceptionMiddleware> _logger;

    public ProblemDetailsExceptionMiddleware(RequestDelegate next, ILogger<ProblemDetailsExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await WriteProblemDetailsAsync(context, ex);
        }
    }

    private async Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
    {
        // If the response has already started, we can't write a new body.
        if (context.Response.HasStarted)
        {
            _logger.LogWarning(ex, "Response has already started, cannot write ProblemDetails.");
            return;
        }

        var (status, title, type) = MapException(ex);

        // Log only server errors as errors (client errors are expected).
        if (status >= 500)
        {
            _logger.LogError(ex, "Unhandled exception.");
        }

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = type,
            Detail = status >= 500 ? "An unexpected error occurred." : ex.Message,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        context.Response.Clear();
        context.Response.StatusCode = status;
        context.Response.ContentType = MediaTypeNames.Application.ProblemJson;

        await context.Response.WriteAsJsonAsync(problem);
    }
    
    private static (int status, string title, string type) MapException(Exception ex)
    {
        // NOTE: Add your project-specific exceptions here.
        return ex switch
        {
            // Application/Common/NotFoundException (or wherever it lives)
            NotFoundException => (StatusCodes.Status404NotFound, "Not Found", "https://httpstatuses.com/404"),

            // Common "bad input" exceptions
            ArgumentException => (StatusCodes.Status400BadRequest, "Bad Request", "https://httpstatuses.com/400"),
            FormatException => (StatusCodes.Status400BadRequest, "Bad Request", "https://httpstatuses.com/400"),
            DomainException => (StatusCodes.Status400BadRequest, "Bad Request", "https://httpstatuses.com/400"),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "https://httpstatuses.com/500")
        };
    }
}
