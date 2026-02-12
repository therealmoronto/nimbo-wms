using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;

namespace Nimbo.Wms.Contracts.Topology;

public static class TopologyExtensions
{
    public static WarehouseListItemDto ToListItemDto(this Warehouse warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        return new WarehouseListItemDto(warehouse.Id, warehouse.Code, warehouse.Name);
    }
    
    public static WarehouseTopologyDto ToTopologyDto(this Warehouse warehouse)
    {
        ArgumentNullException.ThrowIfNull(warehouse);

        var zones = warehouse.Zones.Select(z => z.ToDto()).ToList();
        var locations = warehouse.Locations.Select(l => l.ToDto()).ToList();

        return new WarehouseTopologyDto(warehouse.Id, warehouse.Code, warehouse.Name, warehouse.Address, warehouse.Description, zones, locations);
    }

    public static ZoneDto ToDto(this Zone zone)
    {
        ArgumentNullException.ThrowIfNull(zone);

        return new ZoneDto(
            zone.WarehouseId.Value,
            zone.Id.Value,
            zone.Code,
            zone.Name,
            zone.Type,
            zone.MaxWeightKg,
            zone.MaxVolumeM3,
            zone.IsQuarantine,
            zone.IsDamagedArea
        );
    }
    
    public static LocationDto ToDto(this Location location)
    {
        ArgumentNullException.ThrowIfNull(location);

        return new LocationDto(
            location.WarehouseId.Value,
            location.ZoneId.Value,
            location.Id.Value,
            location.Code,
            location.Type,
            location.MaxWeightKg,
            location.MaxVolumeM3,
            location.IsSingleItemOnly,
            location.IsPickingLocation,
            location.IsReceivingLocation,
            location.IsShippingLocation,
            location.IsActive,
            location.IsBlocked,
            location.Aisle,
            location.Rack,
            location.Level,
            location.Position
        );
    }
}
