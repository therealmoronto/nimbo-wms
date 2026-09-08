using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record PatchAdjustmentDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    Guid? LocationId,
    QuantityDeltaDto? Delta,
    string? Notes,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;
