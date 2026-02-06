using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Topology;

public class EfWarehouseRepository : IWarehouseRepository
{
    private readonly NimboWmsDbContext _dbContext;

    public EfWarehouseRepository(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Warehouse entity, CancellationToken ct = default)
    {
        await _dbContext.Set<Warehouse>().AddAsync(entity, ct);
    }

    public async Task<Warehouse?> GetByIdAsync(WarehouseId id, CancellationToken ct = default)
    {
        return await _dbContext.Set<Warehouse>()
            .Include(x => x.Zones)
            .Include(x => x.Locations)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }

    public Task DeleteAsync(Warehouse entity, CancellationToken ct = default)
    {
        _dbContext.Set<Warehouse>().Remove(entity);
        return Task.CompletedTask;
    }
}
