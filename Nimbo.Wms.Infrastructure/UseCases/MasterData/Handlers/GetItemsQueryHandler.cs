using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IReadOnlyList<ItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Item, ItemDto> _mapper;

    public GetItemsQueryHandler(NimboWmsDbContext dbContext, IMapper<Item, ItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ItemDto>> Handle(GetItemsQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Item>().AsNoTracking();
        var items = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return items;
    }
}
