using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record DeleteCycleCountDocumentLineCommand(
    Guid DocumentId,
    Guid Id,
    long DocumentVersion
) : IRequest, ITxRequest;
