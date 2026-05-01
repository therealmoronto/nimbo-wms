using JetBrains.Annotations;
using Nimbo.Wms.Domain;
using Nimbo.Wms.Domain.Entities;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories;

[PublicAPI]
public interface IRepository
{
}

[PublicAPI]
public interface IEntityRepository<TModel, TId> : IRepository
    where TModel : class, IEntity<TId>
    where TId : struct, IEntityId
{
    Task AddAsync(TModel entity, CancellationToken ct = default);

    Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task DeleteAsync(TModel entity,CancellationToken ct = default);
}

[PublicAPI]
public interface IDocumentRepository<TDocument, TId> : IRepository
    where TDocument : class, IDocument
    where TId : struct, IEntityId
{
    Task AddAsync(TDocument entity, CancellationToken ct = default);

    Task<TDocument?> GetByIdAsync(TId id, CancellationToken ct = default);

    Task DeleteAsync(TDocument entity,CancellationToken ct = default);
}
