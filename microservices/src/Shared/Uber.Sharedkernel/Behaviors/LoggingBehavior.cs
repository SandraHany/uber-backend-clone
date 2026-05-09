using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Sharedkernel.Errors;

namespace Uber.Sharedkernel.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
   ILogger<LoggingBehavior<TRequest, TResponse>> _logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse: Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
       var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName}", requestName);
        var response = await next();
        if (response.IsFailure)
        {
            _logger.LogWarning(
                    "Request {RequestName} failed with error : {@Error}",
                    requestName,
                    response.Error
            );
        }
        else
            _logger.LogInformation("Request {RequestName} handled successfully", requestName);
        return response;
    }
}
