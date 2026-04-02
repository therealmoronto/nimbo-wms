using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record CreateInventoryItemRequest(
    Guid ItemId,
    Guid WarehouseId,
    Guid LocationId,
    Quantity Quantity,
    InventoryStatus Status,
    Guid? BatchId,
    string? SerialNumber,
    decimal? UnitCost
) : IRequest<InventoryItemId>, ITxRequest;

[PublicAPI]
public sealed record CreateInventoryItemResponse(Guid Id);
