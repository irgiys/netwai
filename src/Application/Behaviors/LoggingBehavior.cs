using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var typeName = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName} {@Request}", typeName, request);

        var sw = Stopwatch.StartNew();
        try
        {
            var response = await next(ct);
            sw.Stop();
            logger.LogInformation("Handled {RequestName} in {Elapsed}ms", typeName, sw.ElapsedMilliseconds);
            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            logger.LogError(ex, "Failed {RequestName} after {Elapsed}ms", typeName, sw.ElapsedMilliseconds);
            throw;
        }
    }
}