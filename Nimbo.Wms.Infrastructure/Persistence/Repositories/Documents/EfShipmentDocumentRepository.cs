using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

public sealed class EfShipmentDocumentRepository : EfDocumentRepository<ShipmentDocument, ShipmentDocumentId>, IShipmentDocumentRepository
{
    public EfShipmentDocumentRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<ShipmentDocument?> GetByIdAsync(ShipmentDocumentId id, CancellationToken ct = default)
    {
        return Set.Include(d => d.Lines)
            .Include(d => d.PickLines)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
