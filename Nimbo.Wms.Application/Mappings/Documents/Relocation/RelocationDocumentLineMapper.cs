using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Relocation;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class RelocationDocumentLineMapper : IMapper<RelocationDocumentLine, RelocationDocumentLineDto>
{
    public partial IQueryable<RelocationDocumentLineDto> ProjectToDto(IQueryable<RelocationDocumentLine> items);

    public partial IEnumerable<RelocationDocumentLineDto> MapToDto(IEnumerable<RelocationDocumentLine> items);

    [MapProperty(nameof(RelocationDocumentLine.From), nameof(RelocationDocumentLineDto.FromLocationId))]
    [MapProperty(nameof(RelocationDocumentLine.To), nameof(RelocationDocumentLineDto.ToLocationId))]
    public partial RelocationDocumentLineDto MapToDto(RelocationDocumentLine item);
}
