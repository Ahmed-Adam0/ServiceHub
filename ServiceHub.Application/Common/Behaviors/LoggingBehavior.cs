using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceHub.Application.Common.Models;

namespace ServiceHub.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Starting request {RequestName}", requestName);

        var response = await next();

        if (response.IsFailure)
        {
            _logger.LogWarning("Request {RequestName} failed with error: {ErrorCode} - {ErrorMessage}", 
                requestName, response.Error.Code, response.Error.Message);
        }
        else
        {
            _logger.LogInformation("Request {RequestName} completed successfully", requestName);
        }

        return response;
    }
}
