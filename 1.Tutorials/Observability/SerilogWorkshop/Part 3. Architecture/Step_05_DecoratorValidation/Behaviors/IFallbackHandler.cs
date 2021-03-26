using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Step_05_DecoratorValidation
{
    public interface IFallbackHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleFallback(TRequest request, CancellationToken cancellationToken);
    }
}
