using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Queries;

public sealed class GetInventoryItemsHandler : IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetInventoryItemsHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyList<InventoryItemDto>> HandleAsync(GetInventoryItemsQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<InventoryItem>().AsNoTracking();

        if (query.WarehouseId is not null)
            dbQuery = dbQuery.Where(i => i.WarehouseId == query.WarehouseId);
        
        if (query.ItemId is not null)
            dbQuery = dbQuery.Where(i => i.ItemId == query.ItemId);

        var inventoryItems = await dbQuery.Select(i => new InventoryItemDto(
                i.Id,
                i.ItemId,
                i.WarehouseId,
                i.LocationId,
                i.Quantity,
                i.Status,
                i.BatchId,
                i.SerialNumber,
                i.UnitCost))
            .ToListAsync(ct);
        
        return inventoryItems;
    }
}
