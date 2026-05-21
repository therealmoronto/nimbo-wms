using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.CycleCount;

[PublicAPI]
public sealed record PatchCycleCountDocumentRequest(
    string? Code,
    string? Title,
    long Version
);

[PublicAPI]
public sealed record PatchCycleCountDocumentResponse(Guid Id);
