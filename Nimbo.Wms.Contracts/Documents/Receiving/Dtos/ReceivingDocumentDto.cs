using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Dtos;

[PublicAPI]
public sealed record ReceivingDocumentBodyDto(
    Guid Id,
    Guid WarehouseId,
    Guid SupplierId,
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
public sealed record ReceivingDocumentDto(
    ReceivingDocumentBodyDto Body,
    List<ReceivingDocumentLineDto> Lines
);
