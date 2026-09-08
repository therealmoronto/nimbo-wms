using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record DeleteCycleCountDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    long DocumentVersion
) : IRequest<Result>, ITxRequest;
