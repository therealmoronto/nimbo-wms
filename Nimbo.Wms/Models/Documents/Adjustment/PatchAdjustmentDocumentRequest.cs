using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Adjustment;

[PublicAPI]
public sealed record PatchAdjustmentDocumentRequest(
    string? Code,
    string? Title,
    string? ReasonCode,
    string? ReasonText,
    long Version
);
