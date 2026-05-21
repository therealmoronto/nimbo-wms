using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Shipment;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ShipmentPickLineMapper : IMapper<ShipmentPickLine, ShipmentPickLineDto>
{
    public partial IQueryable<ShipmentPickLineDto> ProjectToDto(IQueryable<ShipmentPickLine> items);

    public partial IEnumerable<ShipmentPickLineDto> MapToDto(IEnumerable<ShipmentPickLine> items);

    public partial ShipmentPickLineDto MapToDto(ShipmentPickLine item);
}
