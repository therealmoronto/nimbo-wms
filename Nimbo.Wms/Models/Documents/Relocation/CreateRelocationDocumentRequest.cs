using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Relocation;

[PublicAPI]
public sealed record CreateRelocationDocumentRequest(
    Guid WarehouseId,
    string Code,
    string Title,
    string? Notes
);

[PublicAPI]
public sealed record CreateRelocationDocumentResponse(Guid DocumentGuid);
