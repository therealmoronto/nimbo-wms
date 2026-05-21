using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record CreateShipmentDocumentRequest(
    Guid WarehouseId,
    string Code,
    string Title
);

[PublicAPI]
public sealed record CreateShipmentDocumentResponse(Guid Id);
