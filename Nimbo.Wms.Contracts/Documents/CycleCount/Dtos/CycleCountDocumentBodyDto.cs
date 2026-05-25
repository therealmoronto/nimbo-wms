using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;

[PublicAPI]
public sealed record CycleCountDocumentBodyDto(
    Guid Id,
    Guid WarehouseId,
    string Code,
    string Title,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? PostedAt,
    long Version
);

[PublicAPI]
public sealed record CycleCountDocumentDto(
    CycleCountDocumentBodyDto Body,
    List<CycleCountDocumentLineDto> Lines
);
