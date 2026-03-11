using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Topology;

[PublicAPI]
[Mapper]
public partial class WarehouseTopologyMapper : IMapper<Warehouse, WarehouseTopologyDto>
{
    public partial IQueryable<WarehouseTopologyDto> ProjectToDto(IQueryable<Warehouse> items);

    public partial IEnumerable<WarehouseTopologyDto> MapToDto(IEnumerable<Warehouse> items);

    public partial WarehouseTopologyDto MapToDto(Warehouse item);
}
