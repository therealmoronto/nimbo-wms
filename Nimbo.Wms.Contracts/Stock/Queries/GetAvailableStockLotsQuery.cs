using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Queries;

[PublicAPI]
public sealed record GetAvailableStockLotsQuery(
    Guid ItemId,
    Guid? WarehouseId,
    Guid? LocationId
) : IRequest<IReadOnlyList<AvailableStockLotDto>>;
