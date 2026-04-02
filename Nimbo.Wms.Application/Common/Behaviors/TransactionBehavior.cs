using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Application.Common.Behaviors;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork uow)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ITxRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();
        await uow.CommitAsync(ct);
        return response;
    }
}
