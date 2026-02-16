using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public class GetItemsHandler : IQueryHandler<GetItemsQuery, IReadOnlyList<ItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetItemsHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ItemDto>> HandleAsync(GetItemsQuery query, CancellationToken ct = default)
    {
        var items = await _dbContext.Set<Item>()
            .AsNoTracking()
            .Select(i => new ItemDto
            (
                i.Id,
                i.Name,
                i.InternalSku,
                i.Barcode,
                i.BaseUomCode,
                i.Manufacturer,
                i.WeightKg,
                i.VolumeM3
            ))
            .ToListAsync(ct);

        return items;
    }
}
