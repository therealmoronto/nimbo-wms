using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;

public interface IVendorLotRepository : IEntityRepository<VendorLot, VendorLotId>
{
    /// <summary>
    /// Finds an existing VendorLot matching the full natural key (ItemId, BatchNumber, SupplierId,
    /// ExpiryDate), or creates and adds a new one if none exists.
    /// </summary>
    Task<VendorLot> FindOrCreateAsync(
        ItemId itemId,
        string batchNumber,
        SupplierId? supplierId,
        DateTime? expiryDate,
        CancellationToken ct = default);
}
