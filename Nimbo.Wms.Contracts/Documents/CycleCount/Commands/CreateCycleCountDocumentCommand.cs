using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.CycleCount.Commands;

[PublicAPI]
public sealed record CreateCycleCountDocumentCommand(
    Guid WarehouseId,
    string Code,
    string Title
) : IRequest<Guid>, ITxRequest;
