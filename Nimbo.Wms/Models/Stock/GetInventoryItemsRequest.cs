using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record GetInventoryItemsRequest(
    Guid? WarehouseId,
    Guid? ItemId,
    Guid? BatchId
);
