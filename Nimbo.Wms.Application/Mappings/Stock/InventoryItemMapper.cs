using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Stock;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class InventoryItemMapper : IMapper<InventoryItem, InventoryItemDto>
{
    public partial IQueryable<InventoryItemDto> ProjectToDto(IQueryable<InventoryItem> items);

    public partial IEnumerable<InventoryItemDto> MapToDto(IEnumerable<InventoryItem> items);

    public partial InventoryItemDto MapToDto(InventoryItem item);
}
