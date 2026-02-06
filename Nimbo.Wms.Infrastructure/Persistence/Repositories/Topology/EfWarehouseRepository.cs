using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Topology;

internal sealed class EfWarehouseRepository : EfRepository<Warehouse, WarehouseId>, IWarehouseRepository
{
    public EfWarehouseRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
