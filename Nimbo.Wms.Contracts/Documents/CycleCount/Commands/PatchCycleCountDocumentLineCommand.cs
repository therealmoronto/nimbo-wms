using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record PatchCycleCountDocumentLineCommand(
    Guid DocumentId,
    Guid LineId,
    QuantityDto? ExpectedQuantity,
    string? Notes,
    long DocumentVersion
) : IRequest, ITxRequest;
