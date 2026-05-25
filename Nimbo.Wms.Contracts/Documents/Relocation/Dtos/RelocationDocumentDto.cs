using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Dtos;

[PublicAPI]
public sealed record RelocationDocumentBodyDto(
    Guid Id,
    Guid WarehouseId,
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
public sealed record RelocationDocumentDto(
    RelocationDocumentBodyDto Body,
    List<RelocationDocumentLineDto> Lines
);
