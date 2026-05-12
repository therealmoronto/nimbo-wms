using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record GetInventoryItemsQuery(
    Guid? WarehouseId,
    Guid? ItemId,
    Guid? BatchId
) : IRequest<IReadOnlyList<InventoryItemDto>>;
