using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Dtos;

[PublicAPI]
public sealed record ShipmentDocumentBodyDto(
    Guid Id,
    Guid WarehouseId,
    Guid? CustomerId,
    string Code,
    string Title,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? PostedAt,
    long Version,
    string? Notes
);

[PublicAPI]
public sealed record ShipmentDocumentDto(
    ShipmentDocumentBodyDto Body,
    List<ShipmentDocumentLineDto> Lines,
    List<ShipmentPickLineDto> PickLines
);
