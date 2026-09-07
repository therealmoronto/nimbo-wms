using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.CycleCount;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class CycleCountDocumentLineMapper : IMapper<CycleCountDocumentLine, CycleCountDocumentLineDto>
{
    public partial IQueryable<CycleCountDocumentLineDto> ProjectToDto(IQueryable<CycleCountDocumentLine> items);

    public partial IEnumerable<CycleCountDocumentLineDto> MapToDto(IEnumerable<CycleCountDocumentLine> items);

    public partial CycleCountDocumentLineDto MapToDto(CycleCountDocumentLine item);
}
