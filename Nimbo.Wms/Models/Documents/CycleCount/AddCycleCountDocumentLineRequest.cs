using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Models.Documents.CycleCount;

[PublicAPI]
public sealed record AddCycleCountDocumentLineRequest(
    Guid CycleCountDocumentId,
    Guid ItemId,
    Guid LocationId,
    QuantityDto ExpectedQuantity,
    Guid? StockLotId,
    string? Notes,
    long DocumentVersion
);

[PublicAPI]
public sealed record AddCycleCountDocumentLineResponse(Guid Id);
