using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Queries;

public sealed class GetInventoryItemsHandler : IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<InventoryItem, InventoryItemDto> _mapper;

    public GetInventoryItemsHandler(NimboWmsDbContext dbContext, IMapper<InventoryItem, InventoryItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<InventoryItemDto>> HandleAsync(GetInventoryItemsQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<InventoryItem>().AsNoTracking();
        if (query.WarehouseId is not null)
            dbQuery = dbQuery.Where(i => i.WarehouseId == query.WarehouseId);

        if (query.ItemId is not null)
            dbQuery = dbQuery.Where(i => i.ItemId == query.ItemId);

        if (query.BatchId is not null)
            dbQuery = dbQuery.Where(i => i.BatchId == query.BatchId);

        var inventoryItems = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);

        return inventoryItems;
    }
}
