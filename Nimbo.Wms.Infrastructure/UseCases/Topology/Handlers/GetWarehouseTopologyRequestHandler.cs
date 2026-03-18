using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Queries;

internal sealed class GetWarehouseTopologyRequestHandler : IRequestHandler<GetWarehouseTopologyRequest, WarehouseTopologyDto>
{
    private readonly NimboWmsDbContext _db;

    public GetWarehouseTopologyRequestHandler(NimboWmsDbContext db)
    {
        _db = db;
    }

    public async Task<WarehouseTopologyDto> Handle(GetWarehouseTopologyRequest request, CancellationToken ct = default)
    {
        var warehouse = await _db.Set<Warehouse>()
            .AsNoTracking()
            .Where(w => w.Id == request.WarehouseId)
            .Select(w => new
            {
                w.Id,
                w.Code,
                w.Name,
                w.Address,
                w.Description,
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
            warehouse.Address,
            warehouse.Description,
            warehouse.Zones.ToList(),
            warehouse.Locations.ToList()
        );
    }
}

