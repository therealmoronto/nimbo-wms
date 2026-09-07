using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Shipment;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ShipmentDocumentLineMapper : IMapper<ShipmentDocumentLine, ShipmentDocumentLineDto>
{
    public partial IQueryable<ShipmentDocumentLineDto> ProjectToDto(IQueryable<ShipmentDocumentLine> items);

    public partial IEnumerable<ShipmentDocumentLineDto> MapToDto(IEnumerable<ShipmentDocumentLine> items);

    public partial ShipmentDocumentLineDto MapToDto(ShipmentDocumentLine item);
}
