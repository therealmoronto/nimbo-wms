using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Contracts;

namespace Nimbo.Wms.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork uow)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ITxRequest
        where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next(ct);
        if (response.IsSuccess)
        {
            await uow.CommitAsync(ct);
        }

        return response;
    }
}
