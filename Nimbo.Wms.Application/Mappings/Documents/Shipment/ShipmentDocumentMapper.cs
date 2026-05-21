using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Shipment.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Shipment;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ShipmentDocumentBodyMapper : IMapper<ShipmentDocument, ShipmentDocumentBodyDto>
{
    public partial IQueryable<ShipmentDocumentBodyDto> ProjectToDto(IQueryable<ShipmentDocument> items);

    public partial IEnumerable<ShipmentDocumentBodyDto> MapToDto(IEnumerable<ShipmentDocument> items);

    public partial ShipmentDocumentBodyDto MapToDto(ShipmentDocument item);
}
