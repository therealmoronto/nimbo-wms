using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record PatchShipmentDocumentRequest(
    Guid? CustomerId,
    string? Code,
    string? Title,
    string? Notes,
    long Version
);
