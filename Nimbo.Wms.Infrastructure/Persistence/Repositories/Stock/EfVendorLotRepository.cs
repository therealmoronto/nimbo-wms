using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;

internal sealed class EfVendorLotRepository : EfEntityRepository<VendorLot, VendorLotId>, IVendorLotRepository
{
    public EfVendorLotRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public async Task<VendorLot> FindOrCreateAsync(
        ItemId itemId,
        string batchNumber,
        SupplierId? supplierId,
        DateTime? expiryDate,
        CancellationToken ct = default)
    {
        var existing = await DbContext.Set<VendorLot>().FirstOrDefaultAsync(v =>
            v.ItemId == itemId
            && v.BatchNumber == batchNumber
            && v.SupplierId == supplierId
            && v.ExpiryDate == expiryDate, ct);

        if (existing is not null)
            return existing;

        var vendorLot = new VendorLot(VendorLotId.New(), itemId, batchNumber, supplierId, expiryDate);
        await AddAsync(vendorLot, ct);
        return vendorLot;
    }
}
