using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories;
using Nimbo.Wms.Domain.Entities.Documents.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories;

public abstract class EfDocumentRepository<TDocument, TId> : IDocumentRepository<TDocument, TId>
    where TDocument : class, IDocument
    where TId : struct, IEntityId
{
    public EfDocumentRepository(NimboWmsDbContext dbContext)
    {
        DbContext = dbContext;
        Set = dbContext.Set<TDocument>();
    }

    protected NimboWmsDbContext DbContext { get; }

    public DbSet<TDocument> Set { get; }

    public virtual Task AddAsync(TDocument entity, CancellationToken ct = default)
    {
        return Set.AddAsync(entity, ct).AsTask();
    }

    public virtual Task<TDocument?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        return Set.FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }

    public virtual Task DeleteAsync(TDocument entity, CancellationToken ct = default)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }
}
