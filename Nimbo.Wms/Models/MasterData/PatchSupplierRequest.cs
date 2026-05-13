using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record PatchSupplierRequest(
    Guid SupplierId,
    string? Code,
    string? Name,
    string? TaxId,
    string? Address,
    string? ContactName,
    string? Phone,
    string? Email,
    bool? IsActive
);
