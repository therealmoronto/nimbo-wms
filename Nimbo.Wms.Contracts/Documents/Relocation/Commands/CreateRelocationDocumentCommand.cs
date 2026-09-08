using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record CreateRelocationDocumentCommand(
    Guid WarehouseId,
    string Code,
    string Title
) : IRequest<Result<Guid>>, ITxRequest;
