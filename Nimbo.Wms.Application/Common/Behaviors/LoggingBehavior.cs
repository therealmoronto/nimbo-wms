using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Nimbo.Wms.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        logger.LogInformation("Starting request {RequestName} {@Request}", requestName, request);

        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await next();

            stopwatch.Stop();
            logger.LogInformation(
                "Finished request {RequestName} in {Elapsed}ms",
                requestName,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception e)
        {
            stopwatch.Stop();
            logger.LogError(
                e,
                "Requested {RequestName} failed after {Elapsed}ms with message: {Message}",
                requestName,
                stopwatch.ElapsedMilliseconds,
                e.Message);

            throw;
        }
    }
}
