using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Topology;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ZoneMapper : IMapper<Zone, ZoneDto>
{
    public partial IQueryable<ZoneDto> ProjectToDto(IQueryable<Zone> zones);

    public partial IEnumerable<ZoneDto> MapToDto(IEnumerable<Zone> zones);

    public partial ZoneDto MapToDto(Zone zone);
}
