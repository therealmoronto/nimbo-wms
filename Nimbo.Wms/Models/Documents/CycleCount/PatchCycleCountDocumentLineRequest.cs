using JetBrains.Annotations;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Models.Documents.CycleCount;

[PublicAPI]
public sealed record PatchCycleCountDocumentLineRequest(
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
);
