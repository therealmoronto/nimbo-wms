using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories;
using Nimbo.Wms.Domain;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories;

internal abstract class EfRepository<TModel, TId> : IRepository<TModel,TId>
    where TModel : class, IEntity<TId>
    where TId : struct, IEntityId
{
    protected EfRepository(NimboWmsDbContext dbContext)
    {
        DbContext = dbContext;
        Set = dbContext.Set<TModel>();
    }

    protected NimboWmsDbContext DbContext { get; }

    protected DbSet<TModel> Set { get; }

    public virtual Task AddAsync(TModel entity, CancellationToken ct = default)
    {
        return Set.AddAsync(entity, ct).AsTask();
    }

    public virtual Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        return Set.FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }

    public virtual Task DeleteAsync(TModel entity, CancellationToken ct = default)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }
}
