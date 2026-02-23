namespace Nimbo.Wms.Contracts.Stock.Http;

public sealed record CreateBatchRequest(
    Guid ItemId,
    string BatchNumber,
    Guid? SupplierId,
    DateTimeOffset? ManufacturedAt,
    DateTimeOffset? ExpiryDate,
    DateTimeOffset? ReceivedAt,
    string? Notes
);
