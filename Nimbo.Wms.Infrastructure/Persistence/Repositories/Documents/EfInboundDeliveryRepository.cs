using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Documents.Inbound;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

internal sealed class EfInboundDeliveryRepository : EfRepository<InboundDelivery, InboundDeliveryId>, IInboundDeliveryRepository
{
    public EfInboundDeliveryRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<InboundDelivery?> GetByIdAsync(InboundDeliveryId id, CancellationToken ct = default)
    {
        return Set.Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }
}
