using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Receiving;

[PublicAPI]
public sealed record PatchReceivingDocumentRequest(
    string? Code,
    string? Title,
    string? Notes,
    long Version
);

[PublicAPI]
public sealed record PatchReceivingDocumentResponse(Guid Id);
