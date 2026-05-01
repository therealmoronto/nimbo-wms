using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record CreateInventoryItemRequest(
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    decimal Quantity,
    string QuantityUom,
    string Status,
    Guid? BatchId,
    string? SerialNumber,
    decimal? UnitCost
) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record CreateInventoryItemResponse(Guid Id);
