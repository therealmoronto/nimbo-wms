using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Adjustment;

[PublicAPI]
public sealed record AddAdjustmentDocumentLineRequest(
    Guid ItemId,
    Guid LocationId,
    QuantityDeltaDto Delta,
    string? Notes,
    long DocumentVersion
);

[PublicAPI]
public sealed record AddAdjustmentDocumentLineResponse(Guid Id);
