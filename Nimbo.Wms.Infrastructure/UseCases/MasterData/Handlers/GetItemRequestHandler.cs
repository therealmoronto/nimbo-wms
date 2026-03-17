using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public sealed class GetItemRequestHandler : IRequestHandler<GetItemRequest, ItemDto>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetItemRequestHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ItemDto> Handle(GetItemRequest request, CancellationToken ct = default)
    {
        var item = await _dbContext.Set<Item>()
            .AsNoTracking()
            .Where(i => i.Id == request.ItemId)
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
            .SingleOrDefaultAsync(ct);

        if (item == null)
            throw new NotFoundException("Item not found");

        return item;
    }
}
