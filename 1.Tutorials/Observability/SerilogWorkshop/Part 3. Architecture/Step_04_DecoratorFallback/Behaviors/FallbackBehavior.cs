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
    public class FallbackBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<IFallbackHandler<TRequest, TResponse>> _fallbackHandlers;

        public FallbackBehavior(ILogger logger, IEnumerable<IFallbackHandler<TRequest, TResponse>> fallbackHandlers)
        {
            _logger = logger;
            _fallbackHandlers = fallbackHandlers;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var fallbackHandler = _fallbackHandlers.FirstOrDefault();
            if (fallbackHandler == null)
            {
                return await next();
            }

            var fallbackPolicy = Policy<TResponse>
                .Handle<Exception>()
                .FallbackAsync(async (token) =>
                {
                    _logger.Warning("{RequestMessage} {@RequestValue} failed. Falling back to {RequestHandler}",
                        request.GetType().FullName,             // Request Message
                        request,                                // Request Value
                        fallbackHandler.GetType().FullName);    // Request Handler

                    return await fallbackHandler.HandleFallback(request, token)
                        .ConfigureAwait(false);
                });

            return await fallbackPolicy.ExecuteAsync(async () => await next());
        }
    }
}
