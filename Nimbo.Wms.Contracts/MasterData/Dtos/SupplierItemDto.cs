namespace Nimbo.Wms.Contracts.Topology.Dtos;

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
