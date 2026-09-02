using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Riok.Mapperly.Abstractions;

namespace Nimbo.Wms.Application.Mappings.Stock;

[PublicAPI]
[Mapper]
public partial class VendorLotMapper : IMapper<VendorLot, VendorLotDto>
{
    public partial IQueryable<VendorLotDto> ProjectToDto(IQueryable<VendorLot> vendorLots);

    public partial IEnumerable<VendorLotDto> MapToDto(IEnumerable<VendorLot> vendorLots);

    public partial VendorLotDto MapToDto(VendorLot vendorLot);
}
