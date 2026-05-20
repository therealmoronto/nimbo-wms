using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Receiving;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ReceivingDocumentBodyMapper : IMapper<ReceivingDocument, ReceivingDocumentBodyDto>
{
    public partial IQueryable<ReceivingDocumentBodyDto> ProjectToDto(IQueryable<ReceivingDocument> items);

    public partial IEnumerable<ReceivingDocumentBodyDto> MapToDto(IEnumerable<ReceivingDocument> items);

    public partial ReceivingDocumentBodyDto MapToDto(ReceivingDocument item);
}
