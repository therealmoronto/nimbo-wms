using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Application.Mappings;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public class GetItemsHandler : IQueryHandler<GetItemsQuery, IReadOnlyList<ItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Item, ItemDto> _mapper;

    public GetItemsHandler(NimboWmsDbContext dbContext, IMapper<Item, ItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ItemDto>> HandleAsync(GetItemsQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Item>().AsNoTracking();
        var items = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return items;
    }
}
