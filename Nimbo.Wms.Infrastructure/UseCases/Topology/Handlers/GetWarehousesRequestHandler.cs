using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Queries;

internal sealed class GetWarehousesRequestHandler : IRequestHandler<GetWarehousesRequest, IReadOnlyList<WarehouseListItemDto>>
{
    private readonly NimboWmsDbContext _db;

    public GetWarehousesRequestHandler(NimboWmsDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<WarehouseListItemDto>> Handle(GetWarehousesRequest request, CancellationToken ct = default)
    {
        return await _db.Set<Warehouse>()
            .AsNoTracking()
            .Select(x => new WarehouseListItemDto(
                x.Id,
                x.Code,
                x.Name
            ))
            .ToListAsync(ct);
    }
}
