using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record AddCycleCountDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid LocationId,
    QuantityDto ExpectedQuantity,
    Guid? StockLotId,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
