using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.CycleCount;

[PublicAPI]
public sealed record CreateCycleCountDocumentRequest(
    Guid WarehouseId,
    string Code,
    string Title
);

[PublicAPI]
public sealed record CreateCycleCountDocumentResponse(Guid Id);
