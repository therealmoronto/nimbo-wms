namespace Nimbo.Wms.Contracts.MasterData.Dtos;

public sealed class SupplierItemDto
{
    public Guid Id { get; set; }

    public Guid SupplierId { get; set; }

    public Guid ItemId { get; set; }

    public string? SupplierSku { get; set; }

    public string? SupplierBarcode { get; set; }

    public decimal? DefaultPurchasePrice { get; set; }

    public string? PurchaseUomCode { get; set; }

    public decimal? UnitsPerPurchaseUom { get; set; }

    public int? LeadTimeDays { get; set; }

    public int? MinOrderQty { get; set; }

    public bool IsPreferred { get; set; }
}
