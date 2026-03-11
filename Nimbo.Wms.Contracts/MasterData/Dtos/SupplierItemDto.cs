using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.MasterData.Dtos;

[PublicAPI]
public sealed record SupplierItemDto(
    Guid Id,
    Guid SupplierId,
    Guid ItemId,
    string? SupplierSku,
    string? SupplierBarcode,
    decimal? DefaultPurchasePrice,
    string? PurchaseUomCode,
    decimal? UnitsPerPurchaseUom,
    int? LeadTimeDays,
    int? MinOrderQty,
    bool IsPreferred
);
