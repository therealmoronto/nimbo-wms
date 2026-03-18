using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

[PublicAPI]
public sealed record WarehouseListItemDto(
    Guid Id,
    string Code,
    string Name,
    string? Address,
    string? Description,
    bool IsActive
);
