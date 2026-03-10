using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.MasterData;

[PublicAPI]
[Mapper]
public partial class SupplierItemMapper : IMapper<SupplierItem, SupplierItemDto>
{
    public partial IQueryable<SupplierItemDto> ProjectToDto(IQueryable<SupplierItem> items);

    public partial IEnumerable<SupplierItemDto> MapToDto(IEnumerable<SupplierItem> items);

    public partial SupplierItemDto MapToDto(SupplierItem item);
}
