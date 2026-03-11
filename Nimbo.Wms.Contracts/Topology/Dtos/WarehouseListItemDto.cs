namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record WarehouseListItemDto(
    Guid Id,
    string Code,
    string Name,
    string? Address,
    string? Description,
    bool IsActive
);
