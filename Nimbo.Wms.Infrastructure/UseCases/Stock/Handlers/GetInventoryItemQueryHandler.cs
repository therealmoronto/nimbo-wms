using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class GetInventoryItemQueryHandler : IRequestHandler<GetInventoryItemQuery, InventoryItemDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<InventoryItem, InventoryItemDto> _mapper;

    public GetInventoryItemQueryHandler(NimboWmsDbContext dbContext, IMapper<InventoryItem, InventoryItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<InventoryItemDto> Handle(GetInventoryItemQuery request, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<InventoryItem>()
            .AsNoTracking()
            .Where(i => i.Id == request.InventoryItemId);

        var inventoryItem = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (inventoryItem is null)
            throw new NotFoundException("Inventory item not found");

        return inventoryItem;
    }
}
