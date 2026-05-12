using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record GetBatchRequest(Guid BatchId);
