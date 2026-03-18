using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

public sealed class GetInventoryItemsHandler : IRequestHandler<GetInventoryItemsRequest, IReadOnlyList<InventoryItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetInventoryItemsHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyList<InventoryItemDto>> Handle(GetInventoryItemsRequest request, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<InventoryItem>().AsNoTracking();

        if (request.WarehouseId is not null)
            dbQuery = dbQuery.Where(i => i.WarehouseId == request.WarehouseId);
        
        if (request.ItemId is not null)
            dbQuery = dbQuery.Where(i => i.ItemId == request.ItemId);

        if (request.BatchId is not null)
            dbQuery = dbQuery.Where(i => i.BatchId == request.BatchId);

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
