using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;

public interface ISupplierRepository : IRepository<Supplier, SupplierId>
{
    Task<Supplier?> GetByIdWithItemsAsync(SupplierId id, CancellationToken ct = default);

    Task<Supplier?> GetByItemIdAsync(SupplierItemId supplierItemId, CancellationToken ct = default);
}
