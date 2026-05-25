using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Adjustment.Commands;

[PublicAPI]
public sealed record PatchAdjustmentDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    Guid? LocationId,
    QuantityDeltaDto? Delta,
    string? Notes,
    long DocumentVersion
) : IRequest, ITxRequest;
