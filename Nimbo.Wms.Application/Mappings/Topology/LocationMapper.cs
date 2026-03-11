using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Topology;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Topology;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class LocationMapper : IMapper<Location, LocationDto>
{
    public partial IQueryable<LocationDto> ProjectToDto(IQueryable<Location> locations);

    public partial IEnumerable<LocationDto> MapToDto(IEnumerable<Location> locations);

    public partial LocationDto MapToDto(Location location);
}
