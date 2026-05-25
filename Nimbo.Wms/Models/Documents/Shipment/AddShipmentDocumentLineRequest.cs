using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Shipment;

[PublicAPI]
public sealed record AddShipmentDocumentLineRequest(
    Guid ItemId,
    QuantityDto RequestedQuantity,
    string? Notes
);

[PublicAPI]
public sealed record AddShipmentDocumentLineResponse(Guid Id);
