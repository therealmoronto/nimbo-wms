using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.Adjustment;

[PublicAPI]
public sealed record PatchAdjustmentDocumentLineRequest(
    Guid? LocationId,
    QuantityDeltaDto? Delta,
    string? Notes,
    long DocumentVersion
);
