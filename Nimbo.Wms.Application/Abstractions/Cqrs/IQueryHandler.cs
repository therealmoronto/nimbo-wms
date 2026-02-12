namespace Nimbo.Wms.Application.Abstractions.Cqrs;

public interface IQueryHandler<in TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken ct = default);
}
