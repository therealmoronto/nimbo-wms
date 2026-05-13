using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record DeleteWarehouseRequest(Guid WarehouseId);
