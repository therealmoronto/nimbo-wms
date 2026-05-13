using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record DeleteLocationRequest(Guid LocationGuid);
