using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;

[PublicAPI]
public sealed record AdjustmentDocumentBodyDto(
    Guid Id,
    Guid WarehouseId,
    string Code,
    string Title,
    string ReasonCode,
    string? ReasonText,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? PostedAt,
    long Version
);

[PublicAPI]
public sealed record AdjustmentDocumentDto(
    AdjustmentDocumentBodyDto Body,
    List<AdjustmentDocumentLineDto> Lines
);
