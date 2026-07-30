using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;

[PublicAPI]
public sealed record AdjustmentDocumentLineDto(
    Guid Id,
    Guid DocumentId,
    Guid LocationId,
    Guid ItemId,
    Guid? BatchId,
    QuantityDeltaDto Delta,
    string? Notes
);
