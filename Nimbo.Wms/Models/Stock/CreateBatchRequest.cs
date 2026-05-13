using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record CreateBatchRequest(
    Guid ItemId,
    Guid? SupplierId,
    string BatchNumber,
    DateTimeOffset? ManufacturedAt,
    DateTimeOffset? ExpiryDate,
    DateTimeOffset? ReceivedAt,
    string? Notes
);

[PublicAPI]
public sealed record CreateBatchResponse(Guid Id);
