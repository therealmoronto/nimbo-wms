using JetBrains.Annotations;

namespace Nimbo.Wms.Infrastructure.Integrations;

[PublicAPI]
public interface IErpIntegrationService
{
    Task NotifyEventAsync(string eventType, string payload, CancellationToken ct);
}
