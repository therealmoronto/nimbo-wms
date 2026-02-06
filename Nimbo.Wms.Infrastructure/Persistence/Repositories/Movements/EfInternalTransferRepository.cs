using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Movements;
using Nimbo.Wms.Domain.Entities.Movements;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Movements;

internal sealed class EfInternalTransferRepository : EfRepository<InternalTransfer, InternalTransferId>, IInternalTransferRepository
{
    public EfInternalTransferRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }
}
