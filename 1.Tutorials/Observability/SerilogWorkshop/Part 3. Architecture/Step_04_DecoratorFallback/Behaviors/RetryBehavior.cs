using MediatR;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Step_04_DecoratorFallback
{
    public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IRetryRequest<TRequest, TResponse>> _retryHandlers;

        public RetryBehavior(ILogger logger, IEnumerable<IRetryRequest<TRequest, TResponse>> retryHandlers)
        {
            _logger = logger;
            _retryHandlers = retryHandlers;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var retryHandler = _retryHandlers.FirstOrDefault();
            if (retryHandler == null)
            {
                return await next();
            }

            var retryPolicy = Policy<TResponse>
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: retryHandler.RetryAttempts,
                    sleepDurationProvider: retryAttempt =>
                        {
                            // 1 = 2, 4, 8, 16, ...
                            var retryDelay = retryHandler.RetryWithExponentialBackoff
                                ? TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * retryHandler.RetryDelaySeconds)
                                : TimeSpan.FromSeconds(retryHandler.RetryDelaySeconds);

                            return retryDelay;
                        },
                    onRetry: (result, retryDelay, retryAttempt, context) =>
                        {
                            _logger.Warning(result.Exception, "{RequestMessage} {@RequestValue} is Retrying {RetryAttempt}, Retry to {RequestHandler} for waiting {RetryDelay}",
                                request.GetType().FullName,         // Request Message
                                request,                            // Request Value
                                retryAttempt,                       // RetryAttempt
                                retryHandler.GetType().FullName,    // Request Handler
                                retryDelay);                        // RetryDelay
                        });

            var response = await retryPolicy
                .ExecuteAsync(async () => await next());

            return response;
        }
    }
}
