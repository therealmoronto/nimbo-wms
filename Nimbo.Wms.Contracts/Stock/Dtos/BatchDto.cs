using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

[PublicAPI]
public sealed record BatchDto(
    Guid Id,
    Guid ItemId,
    string BatchNumber,
    Guid? SupplierId,
    DateTime? ManufacturedAt,
    DateTime? ExpiryDate,
    DateTime? ReceivedAt,
    string? Notes
);
