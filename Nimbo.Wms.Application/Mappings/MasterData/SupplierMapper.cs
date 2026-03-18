using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.MasterData;

[PublicAPI]
[Mapper]
public partial class SupplierMapper : IMapper<Supplier, SupplierDto>
{
    public partial IQueryable<SupplierDto> ProjectToDto(IQueryable<Supplier> suppliers);

    public partial IEnumerable<SupplierDto> MapToDto(IEnumerable<Supplier> suppliers);

    public partial SupplierDto MapToDto(Supplier supplier);
}
