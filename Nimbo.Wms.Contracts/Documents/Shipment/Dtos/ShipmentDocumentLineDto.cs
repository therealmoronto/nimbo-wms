using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Dtos;

[PublicAPI]
public sealed record ShipmentDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid ItemId,
    Guid? BatchId,
    QuantityDto RequestedQuantity,
    string? Notes
);
