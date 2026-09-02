using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

/// <summary>
/// A stock lot with remaining quantity at a specific item/location, for manual FIFO/FEFO pick selection.
/// Ordered by VendorLot.ExpiryDate ascending when present (FEFO), else StockLot.ReceivedAt ascending (FIFO).
/// </summary>
[PublicAPI]
public sealed record AvailableStockLotDto(
    Guid StockLotId,
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    Guid? VendorLotId,
    string? VendorLotBatchNumber,
    DateTime? VendorLotExpiryDate,
    DateTime ReceivedAt,
    QuantityDto RemainingQuantity
);
