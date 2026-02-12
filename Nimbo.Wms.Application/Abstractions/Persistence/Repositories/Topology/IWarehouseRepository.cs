using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;

public interface IWarehouseRepository : IRepository<Warehouse, WarehouseId>
{
    Task<Warehouse?> GetByLocationIdAsync(LocationId locationId, CancellationToken ct = default);

    Task<Warehouse?> GetByZoneIdAsync(ZoneId zoneId, CancellationToken ct = default);
}
