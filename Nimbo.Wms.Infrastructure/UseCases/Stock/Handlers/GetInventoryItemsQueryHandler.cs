using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class GetInventoryItemsQueryHandler : IRequestHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<InventoryItem, InventoryItemDto> _mapper;

    public GetInventoryItemsQueryHandler(NimboWmsDbContext dbContext, IMapper<InventoryItem, InventoryItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<InventoryItemDto>> Handle(GetInventoryItemsQuery query, CancellationToken ct = default)
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
