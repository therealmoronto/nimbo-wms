using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.CycleCount;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class CycleCountDocumentBodyMapper : IMapper<CycleCountDocument, CycleCountDocumentBodyDto>
{
    public partial IQueryable<CycleCountDocumentBodyDto> ProjectToDto(IQueryable<CycleCountDocument> items);

    public partial IEnumerable<CycleCountDocumentBodyDto> MapToDto(IEnumerable<CycleCountDocument> items);

    public partial CycleCountDocumentBodyDto MapToDto(CycleCountDocument item);
}
