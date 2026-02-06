using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;

internal sealed class EfSupplierRepository : EfRepository<Supplier, SupplierId>, ISupplierRepository
{
    public EfSupplierRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<Supplier?> GetByIdAsync(SupplierId id, CancellationToken ct = default)
    {
        return Set.Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), ct);
    }
}
