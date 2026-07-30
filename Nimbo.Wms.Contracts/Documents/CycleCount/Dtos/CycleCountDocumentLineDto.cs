using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;

[PublicAPI]
public sealed record CycleCountDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid LocationId,
    Guid ItemId,
    Guid? BatchId,
    QuantityDto ExpectedQuantity,
    QuantityDto? ActualQuantity,
    string? Notes
);
