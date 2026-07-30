using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.CycleCount;

[PublicAPI]
public sealed record PatchCycleCountDocumentLineRequest(
    Guid? BatchId,
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
);
