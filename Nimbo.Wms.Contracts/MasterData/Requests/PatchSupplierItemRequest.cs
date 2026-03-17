using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record PatchSupplierItemRequest(
    Guid SupplierGuid,
    Guid SupplierItemGuid,
    string? SupplierSku,
    string? SupplierBarcode,
    decimal? DefaultPurchasePrice,
    string? PurchaseUomCode,
    decimal? UnitsPerPurchaseUom,
    int? LeadTimeDays,
    int? MinOrderQty,
    bool? IsPreferred
) : IRequest;
