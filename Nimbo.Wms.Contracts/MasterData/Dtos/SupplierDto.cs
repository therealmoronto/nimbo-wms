namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record SupplierDto(
    Guid Id,
    string Code,
    string Name,
    string? TaxId,
    string? Address,
    string? ContactName,
    string? Phone,
    string? Email,
    bool IsActive,
    List<SupplierItemDto> Items
);
