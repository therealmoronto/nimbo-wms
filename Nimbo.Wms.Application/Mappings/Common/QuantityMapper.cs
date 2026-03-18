using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;
using Nimbo.Wms.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Common;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class QuantityMapper : IValueObjectMapper<Quantity, QuantityDto>
{
    public partial IEnumerable<QuantityDto> MapToDto(IEnumerable<Quantity> items);

    public partial QuantityDto MapToDto(Quantity item);
}
