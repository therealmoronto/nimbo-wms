using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public class GetItemsRequestHandler : IRequestHandler<GetItemsRequest, IReadOnlyList<ItemDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetItemsRequestHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ItemDto>> Handle(GetItemsRequest request, CancellationToken ct = default)
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
