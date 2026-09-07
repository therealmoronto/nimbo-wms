using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Receiving;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ReceivingDocumentLineMapper : IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto>
{
    public partial IQueryable<ReceivingDocumentLineDto> ProjectToDto(IQueryable<ReceivingDocumentLine> items);

    public partial IEnumerable<ReceivingDocumentLineDto> MapToDto(IEnumerable<ReceivingDocumentLine> items);

    public partial ReceivingDocumentLineDto MapToDto(ReceivingDocumentLine item);
}
