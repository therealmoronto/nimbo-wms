using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Documents.Outbound;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

internal sealed class EfShipmentOrderRepository : EfRepository<ShipmentOrder, ShipmentOrderId>, IShipmentOrderRepository
{
    public EfShipmentOrderRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<ShipmentOrder?> GetByIdAsync(ShipmentOrderId id, CancellationToken ct = default)
    {
        return Set.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }
}
