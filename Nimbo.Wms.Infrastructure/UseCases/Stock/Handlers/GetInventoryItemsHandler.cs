using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

public sealed class GetInventoryItemsHandler : IRequestHandler<GetInventoryItemsRequest, IReadOnlyList<InventoryItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<InventoryItem, InventoryItemDto> _mapper;

    public GetInventoryItemsHandler(NimboWmsDbContext dbContext, IMapper<InventoryItem, InventoryItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
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

        var inventoryItems = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);

        return inventoryItems;
    }
}
