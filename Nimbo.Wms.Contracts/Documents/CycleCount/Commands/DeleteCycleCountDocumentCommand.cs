using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record DeleteCycleCountDocumentCommand(
    Guid Id,
    long Version
) : IRequest<Result>, ITxRequest;
