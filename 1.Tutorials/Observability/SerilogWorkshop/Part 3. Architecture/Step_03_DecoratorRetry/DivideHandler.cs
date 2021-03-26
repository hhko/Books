using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Step_03_DecoratorRetry
{
    public class DivideHandler : IRequestHandler<Divide, Unit>, IRetryRequest<Divide, Unit>
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
    }
}
