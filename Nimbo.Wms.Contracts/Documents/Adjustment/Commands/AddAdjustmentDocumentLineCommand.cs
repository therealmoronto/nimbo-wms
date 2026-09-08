using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record AddAdjustmentDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid LocationId,
    QuantityDeltaDto Delta,
    Guid? StockLotId,
    string? Notes,
    long DocumentVersion
) : IRequest<Result<Guid>>, ITxRequest;
