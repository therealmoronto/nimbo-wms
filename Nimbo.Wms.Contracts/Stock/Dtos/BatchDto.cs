namespace Nimbo.Wms.Contracts.Stock.Dtos;

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
