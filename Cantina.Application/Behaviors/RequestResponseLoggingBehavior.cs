using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cantina.Application.Behaviors
{
    public class RequestResponseLoggingBehavior<TRequest, TResponse>(ILogger<RequestResponseLoggingBehavior<TRequest, TResponse>> logger): IPipelineBehavior<TRequest, TResponse>
    where TRequest : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            var requestJson = JsonSerializer.Serialize(request);
            logger.LogInformation("Handling request {CorrelationID}: {Request}", correlationId, requestJson);
            var response = await next(cancellationToken);
            var responseJson = JsonSerializer.Serialize(response);
            logger.LogInformation("Response for {Correlation}: {Response}", correlationId, responseJson);
            return response;
        }
    }
}
