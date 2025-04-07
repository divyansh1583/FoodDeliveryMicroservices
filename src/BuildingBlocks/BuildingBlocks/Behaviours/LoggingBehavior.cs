using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> (ILogger<LoggingBehavior<TRequest,TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest :notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            var requestName = typeof(TRequest).Name;
            var responseName = typeof(TResponse).Name;

            logger.LogInformation("[Start] Handling request={Request} - Response={@Request}", requestName, responseName);
            var timer = new Stopwatch();

            timer.Start();
            var response = await next();
            timer.Stop();

            var timeTaken = timer.Elapsed;

            if (timeTaken.Seconds > 3)
                logger.LogWarning("[Performance] Long running request={Request} - Response={@Request} - TimeTaken = {TimeTaken} seconds", requestName, responseName, timeTaken.Seconds);

            logger.LogInformation("[End] Handled request={Request} - Response={@Request} - TimeTaken={TimeTaken}", requestName, responseName, timeTaken);
            return response;
        }
    }
}
