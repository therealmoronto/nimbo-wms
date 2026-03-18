using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Common.Dtos;
using Nimbo.Wms.Domain.ValueObject;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Common;

[PublicAPI]
[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public partial class QunatityDeltaMapper : IValueObjectMapper<QuantityDelta, QuantityDeltaDto>
{
    public partial IEnumerable<QuantityDeltaDto> MapToDto(IEnumerable<QuantityDelta> items);

    public partial QuantityDeltaDto MapToDto(QuantityDelta item);
}
