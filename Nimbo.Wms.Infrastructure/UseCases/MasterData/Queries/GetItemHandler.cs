using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public sealed class GetItemHandler : IQueryHandler<GetItemQuery, ItemDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Item, ItemDto> _mapper;

    public GetItemHandler(NimboWmsDbContext dbContext, IMapper<Item, ItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ItemDto> HandleAsync(GetItemQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Item>()
            .AsNoTracking()
            .Where(i => i.Id == query.ItemId);

        var item = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);

        if (item == null)
            throw new NotFoundException("Item not found");

        return item;
    }
}
