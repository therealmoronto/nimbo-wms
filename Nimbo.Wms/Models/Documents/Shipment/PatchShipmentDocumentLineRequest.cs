using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record PatchShipmentDocumentLineRequest(
    QuantityDto? RequestedQuantity,
    string? Notes,
    long DocumentVersion
);
