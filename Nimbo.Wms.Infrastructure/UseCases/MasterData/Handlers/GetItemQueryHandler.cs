using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Queries;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class GetItemQueryHandler : IRequestHandler<GetItemQuery, Result<ItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Item, ItemDto> _mapper;

    public GetItemQueryHandler(NimboWmsDbContext dbContext, IMapper<Item, ItemDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<ItemDto>> Handle(GetItemQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Item>()
            .AsNoTracking()
            .Where(i => i.Id == query.ItemId);

        var item = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);

        if (item == null)
        {
            return Error.NotFound("item.notfound", "Item not found");
        }

        return item;
    }
}
