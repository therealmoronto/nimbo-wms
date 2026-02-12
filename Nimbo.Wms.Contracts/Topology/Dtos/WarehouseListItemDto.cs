using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Topology.Dtos;

public sealed record WarehouseListItemDto(
    Guid Id,
    string Code,
    string Name
);
