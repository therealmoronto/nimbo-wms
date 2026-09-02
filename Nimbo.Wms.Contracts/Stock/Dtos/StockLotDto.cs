using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

[PublicAPI]
public sealed record StockLotDto(
    Guid Id,
    Guid ItemId,
    Guid ReceivingDocumentId,
    DateTime ReceivedAt,
    Guid? VendorLotId,
    decimal? UnitCost
);
