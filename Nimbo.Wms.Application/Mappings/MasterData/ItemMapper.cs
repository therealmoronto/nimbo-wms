using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.MasterData;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class ItemMapper : IMapper<Item, ItemDto>
{
    public partial ItemDto MapToDto(Item item);

    public partial IQueryable<ItemDto> ProjectToDto(IQueryable<Item> items);
}
