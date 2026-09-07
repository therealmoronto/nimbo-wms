using JetBrains.Annotations;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Documents.Adjustment;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class AdjustmentDocumentBodyMapper : IMapper<AdjustmentDocument, AdjustmentDocumentBodyDto>
{
    public partial IQueryable<AdjustmentDocumentBodyDto> ProjectToDto(IQueryable<AdjustmentDocument> items);

    public partial IEnumerable<AdjustmentDocumentBodyDto> MapToDto(IEnumerable<AdjustmentDocument> items);

    public partial AdjustmentDocumentBodyDto MapToDto(AdjustmentDocument item);
}
