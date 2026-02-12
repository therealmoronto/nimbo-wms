using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Queries;

internal sealed class GetWarehouseTopologyHandler : IQueryHandler<GetWarehouseTopologyQuery, WarehouseTopologyDto>
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
                w.Id,
                w.Code,
                w.Name,
                Zones = w.Zones.Select(z => new ZoneDto(
                    z.WarehouseId.Value,
                    z.Id.Value,
                    z.Code,
                    z.Name,
                    z.Type,
                    z.MaxWeightKg,
                    z.MaxVolumeM3,
                    z.IsQuarantine,
                    z.IsDamagedArea
                )),
                Locations = w.Locations.Select(l => new LocationDto(
                    l.WarehouseId.Value,
                    l.ZoneId.Value,
                    l.Id.Value,
                    l.Code,
                    l.Type,
                    l.MaxWeightKg,
                    l.MaxVolumeM3,
                    l.IsSingleItemOnly,
                    l.IsPickingLocation,
                    l.IsReceivingLocation,
                    l.IsShippingLocation,
                    l.IsActive,
                    l.IsBlocked,
                    l.Aisle,
                    l.Rack,
                    l.Level,
                    l.Position
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

