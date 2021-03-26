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
    public class DivideHandler : IRequestHandler<Divide, Unit>, IRetryRequest<Divide, Unit>, IFallbackHandler<Divide, Unit>
    {
        private ILogger _logger;

        public DivideHandler(ILogger logger)
        {
            _logger = logger;
        }

        public int RetryAttempts => 3;

        public int RetryDelaySeconds => 1;

        public bool RetryWithExponentialBackoff => true;

        public Task<Unit> Handle(Divide request, CancellationToken cancellationToken)
        {
            int result = request.X / request.Y;
            _logger.Information("Divide is {Result}", result);

            return Unit.Task;
        }

        public Task<Unit> HandleFallback(Divide request, CancellationToken cancellationToken)
        {
            //
            // Handle 예외 발생시 호출한다.
            //
            _logger.Information("Fallback is excecuted.");

            //throw new DivideByZeroException("또 예외. 환장하겠네(I'm going nuts).");

            return Unit.Task;
        }
    }
}
