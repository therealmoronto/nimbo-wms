using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Stock.Commands;

[PublicAPI]
public sealed record CreateInventoryItemCommand(
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    decimal Quantity,
    string QuantityUom,
    string Status,
    Guid StockLotId,
    string? SerialNumber,
    decimal? UnitCost
) : IRequest<Result<Guid>>, ITxRequest;
