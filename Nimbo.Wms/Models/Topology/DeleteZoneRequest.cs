using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Topology;

[PublicAPI]
public sealed record DeleteZoneRequest(Guid ZoneGuid);
