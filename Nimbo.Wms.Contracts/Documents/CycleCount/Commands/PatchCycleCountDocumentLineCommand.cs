using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record PatchCycleCountDocumentLineCommand(
    Guid DocumentId,
    Guid LineId,
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;
