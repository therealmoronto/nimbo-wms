using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Relocation;

[PublicAPI]
public sealed record PatchRelocationDocumentRequest(
    string? Code,
    string? Title,
    string? Notes,
    long Version
);
