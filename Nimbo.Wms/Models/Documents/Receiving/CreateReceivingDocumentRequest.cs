using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Receiving;

[PublicAPI]
public sealed record CreateReceivingDocumentRequest(
    Guid WarehouseId,
    Guid SupplierId,
    string Code,
    string Title,
    string? Notes
);

[PublicAPI]
public sealed record CreateReceivingDocumentResponse(Guid Id);
