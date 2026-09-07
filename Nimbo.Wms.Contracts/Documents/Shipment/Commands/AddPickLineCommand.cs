using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record AddPickLineCommand(
    Guid DocumentId,
    Guid ItemId,
    Guid FromLocationId,
    Guid StockLotId,
    QuantityDto Quantity,
    string? Notes,
    long DocumentVersion
) : IRequest<Guid>, ITxRequest;