using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Adjustment;

[PublicAPI]
public sealed record CreateAdjustmentDocumentRequest(
    Guid WarehouseId,
    string Code,
    string Title,
    string ReasonCode,
    string? ReasonText
);

[PublicAPI]
public sealed record CreateAdjustmentDocumentResponse(Guid Id);
