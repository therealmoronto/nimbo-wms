using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Stock.Dtos;

[PublicAPI]
public sealed record VendorLotDto(
    Guid Id,
    Guid ItemId,
    string BatchNumber,
    Guid? SupplierId,
    DateTime? ExpiryDate
);
