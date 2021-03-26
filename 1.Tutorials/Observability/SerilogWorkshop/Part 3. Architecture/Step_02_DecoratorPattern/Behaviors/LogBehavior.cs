using MediatR;
using Serilog;
using SerilogTimings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Step_02_DecoratorPattern
{
    public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private ILogger _logger;

        public LogBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var op = Operation.Begin("{Request}", request.GetType().FullName))
            {
                try
                {
                    var result = await next();
                    op.Complete();
                    return result;
                }
                catch
                {
                    op.Abandon();
                    throw;
                }
            }
        }
    }
}
