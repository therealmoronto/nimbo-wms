using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record DeleteCycleCountDocumentCommand(
    Guid Id,
    long Version
) : IRequest, ITxRequest;
