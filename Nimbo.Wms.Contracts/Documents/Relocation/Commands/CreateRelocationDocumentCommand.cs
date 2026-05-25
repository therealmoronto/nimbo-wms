using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record CreateRelocationDocumentCommand(
    Guid WarehouseId,
    string Code,
    string Title
) : IRequest<Guid>, ITxRequest;
