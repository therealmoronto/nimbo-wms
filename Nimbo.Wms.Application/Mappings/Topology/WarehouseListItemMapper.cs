using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Topology;

[PublicAPI]
[Mapper]
public partial class WarehouseListItemMapper : IMapper<Warehouse, WarehouseListItemDto>
{
    public partial IQueryable<WarehouseListItemDto> ProjectToDto(IQueryable<Warehouse> items);

    public partial IEnumerable<WarehouseListItemDto> MapToDto(IEnumerable<Warehouse> items);

    [MapperIgnoreSource(nameof(Warehouse.Zones))]
    [MapperIgnoreSource(nameof(Warehouse.Locations))]
    public partial WarehouseListItemDto MapToDto(Warehouse item);
}
