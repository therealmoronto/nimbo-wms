using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.ValueObject;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record AddShipmentDocumentLineCommand(
    Guid DocumentId,
    Guid ItemId,
    QuantityDto RequestedQuantity,
    Guid? StockLotId,
    string? Notes,
    long DocumentVersion
) : IRequest<Result<Guid>>, ITxRequest;
