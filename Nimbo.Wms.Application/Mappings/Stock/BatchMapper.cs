using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Stock;

[PublicAPI]
[Mapper]
public partial class BatchMapper : IMapper<Batch, BatchDto>
{
    public partial IQueryable<BatchDto> ProjectToDto(IQueryable<Batch> batches);

    public partial IEnumerable<BatchDto> MapToDto(IEnumerable<Batch> batches);

    public partial BatchDto MapToDto(Batch batch);
}
