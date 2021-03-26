using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Step_04_DecoratorFallback
{
    public interface IRetryRequest<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        int RetryAttempts { get; }

        int RetryDelaySeconds { get; }

        bool RetryWithExponentialBackoff { get; }
    }
}
