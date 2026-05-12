using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record GetBatchesRequest(Guid? ItemId, Guid? SupplierId);
