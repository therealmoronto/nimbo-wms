using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;

[PublicAPI]
public sealed record CycleCountDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid LocationId,
    Guid ItemId,
    QuantityDto ExpectedQuantity,
    QuantityDto? ActualQuantity,
    string? Notes
);
