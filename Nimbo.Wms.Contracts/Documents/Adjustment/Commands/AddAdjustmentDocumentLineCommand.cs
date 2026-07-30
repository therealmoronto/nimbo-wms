using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record AddAdjustmentDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid? BatchId,
    Guid LocationId,
    QuantityDeltaDto Delta,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
