using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Queries;

public sealed class GetWarehouseTopologyHandler : IQueryHandler<GetWarehouseTopologyQuery, WarehouseTopologyDto>
{
    private readonly NimboWmsDbContext _db;

    public GetWarehouseTopologyHandler(NimboWmsDbContext db)
    {
        _db = db;
    }

    public async Task<WarehouseTopologyDto> HandleAsync(
        GetWarehouseTopologyQuery query,
        CancellationToken ct = default)
    {
        var warehouse = await _db.Set<Warehouse>()
            .AsNoTracking()
            .Where(x => x.Id.Equals(query.WarehouseId))
            .Select(x => new
            {
                x.Id,
                x.Code,
                x.Name,
                Zones = x.Zones.Select(z => new ZoneDto(
                    z.Id,
                    z.Code,
                    z.Name
                )),
                Locations = x.Locations.Select(l => new LocationDto(
                    l.Id,
                    l.ZoneId,
                    l.Code
                ))
            })
            .SingleOrDefaultAsync(ct);

        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        return new WarehouseTopologyDto(
            warehouse.Id,
            warehouse.Code,
            warehouse.Name,
            warehouse.Zones.ToList(),
            warehouse.Locations.ToList()
        );
    }
}

