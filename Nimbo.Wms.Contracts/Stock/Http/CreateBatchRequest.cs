namespace Nimbo.Wms.Contracts.Stock.Http;

public sealed record CreateBatchRequest(
    Guid ItemId,
    string BatchNumber,
    Guid? SupplierId,
    DateTime? ManufacturedAt,
    DateTime? ExpiryDate,
    DateTime? ReceivedAt,
    string? Notes
);
