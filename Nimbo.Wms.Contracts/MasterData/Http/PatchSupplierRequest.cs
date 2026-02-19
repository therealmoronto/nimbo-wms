namespace Nimbo.Wms.Contracts.MasterData.Http;

public sealed record PatchSupplierRequest(
    string? Code,
    string? Name,
    string? TaxId,
    string? Address,
    string? ContactName,
    string? Phone,
    string? Email,
    bool? IsActive
);
