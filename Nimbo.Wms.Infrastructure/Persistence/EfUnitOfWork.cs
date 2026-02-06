using Nimbo.Wms.Application.Abstractions.Persistence;

namespace Nimbo.Wms.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly NimboWmsDbContext _dbContext;

    public EfUnitOfWork(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _dbContext.SaveChangesAsync(ct);
}
