using Nimbo.Wms.Domain;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories;

public interface IRepository
{
}

public interface IRepository<TModel, TId> : IRepository
    where TModel : class, IEntity<TId>
    where TId : struct, IEntityId
{
    Task AddAsync(TModel entity, CancellationToken ct = default);

    Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task DeleteAsync(TModel entity,CancellationToken ct = default);
}
