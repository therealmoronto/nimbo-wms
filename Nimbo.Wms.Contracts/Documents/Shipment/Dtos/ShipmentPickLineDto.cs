using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Dtos;

[PublicAPI]
public sealed record ShipmentPickLineDto(
    Guid Id,
    Guid DocumentId,
    Guid ItemId,
    Guid FromLocation,
    QuantityDto Quantity,
    string? Notes
);
