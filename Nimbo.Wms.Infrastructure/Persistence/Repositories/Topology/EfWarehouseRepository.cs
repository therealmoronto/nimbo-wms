using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Topology;

internal sealed class EfWarehouseRepository : EfRepository<Warehouse, WarehouseId>, IWarehouseRepository
{
    public EfWarehouseRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
    
    public override Task<Warehouse?> GetByIdAsync(WarehouseId id, CancellationToken ct = default)
    {
        return Set.Include(w => w.Zones)
            .Include(w => w.Locations)
            .FirstOrDefaultAsync(w => w.Id.Equals(id), ct);
    }

    public Task<Warehouse?> GetByLocationIdAsync(LocationId locationId, CancellationToken ct = default)
    {
        return Set.Include(w => w.Zones)
            .Include(w => w.Locations)
            .FirstOrDefaultAsync(w => w.Locations.Any(l => l.Id == locationId), ct);
    }

    public Task<Warehouse?> GetByZoneIdAsync(ZoneId zoneId, CancellationToken ct = default)
    {
        return Set.Include(w => w.Zones)
            .Include(w => w.Locations)
            .FirstOrDefaultAsync(w => w.Zones.Any(z => z.Id == zoneId), ct);
    }
}
