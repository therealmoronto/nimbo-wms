using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Relocation;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class RelocationDocumentBodyMapper : IMapper<RelocationDocument, RelocationDocumentBodyDto>
{
    public partial IQueryable<RelocationDocumentBodyDto> ProjectToDto(IQueryable<RelocationDocument> items);

    public partial IEnumerable<RelocationDocumentBodyDto> MapToDto(IEnumerable<RelocationDocument> items);

    public partial RelocationDocumentBodyDto MapToDto(RelocationDocument item);
}
