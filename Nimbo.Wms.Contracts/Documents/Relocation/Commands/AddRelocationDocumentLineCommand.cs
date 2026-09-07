using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Commands;

[PublicAPI]
public sealed record AddRelocationDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid FromLocationId,
    Guid ToLocationId,
    QuantityDto Quantity,
    Guid? StockLotId,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;
