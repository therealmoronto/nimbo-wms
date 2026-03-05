using Nimbo.Wms.Application.Abstractions.Persistence;

namespace Nimbo.Wms.Infrastructure.Persistence;

internal sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly NimboWmsDbContext _dbContext;

    public EfUnitOfWork(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task CommitAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);
}
