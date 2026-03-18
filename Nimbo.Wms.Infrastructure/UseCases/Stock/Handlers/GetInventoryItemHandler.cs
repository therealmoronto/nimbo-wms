using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

public sealed class GetInventoryItemHandler : IRequestHandler<GetInventoryItemRequest, InventoryItemDto>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetInventoryItemHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventoryItemDto> Handle(GetInventoryItemRequest request, CancellationToken ct = default)
    {
        var inventoryItem = await _dbContext.Set<InventoryItem>()
            .AsNoTracking()
            .Where(i => i.Id == request.InventoryItemId)
            .Select(i => new InventoryItemDto(
                i.Id,
                i.ItemId,
                i.WarehouseId,
                i.LocationId,
                i.Quantity,
                i.Status,
                i.BatchId,
                i.SerialNumber,
                i.UnitCost))
            .SingleOrDefaultAsync(ct);

        if (inventoryItem is null)
            throw new NotFoundException("Inventory item not found");

        return inventoryItem;
    }
}
