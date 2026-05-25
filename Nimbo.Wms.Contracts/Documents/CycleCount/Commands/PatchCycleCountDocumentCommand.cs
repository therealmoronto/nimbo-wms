using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record PatchCycleCountDocumentCommand(
    Guid Id,
    string? Code,
    string? Title,
    long Version
) : IRequest, ITxRequest;
