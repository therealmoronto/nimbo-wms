using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Adjustment;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class AdjustmentDocumentLineMapper : IMapper<AdjustmentDocumentLine, AdjustmentDocumentLineDto>
{
    public partial IQueryable<AdjustmentDocumentLineDto> ProjectToDto(IQueryable<AdjustmentDocumentLine> items);

    public partial IEnumerable<AdjustmentDocumentLineDto> MapToDto(IEnumerable<AdjustmentDocumentLine> items);

    [MapperIgnoreSource(nameof(AdjustmentDocumentLine.Quantity))]
    public partial AdjustmentDocumentLineDto MapToDto(AdjustmentDocumentLine item);
}
