using System.Text;
using JetBrains.Annotations;

namespace Nimbo.Wms.Infrastructure.Integrations;

[PublicAPI]
internal sealed class ErpIntegrationService(HttpClient httpClient) : IErpIntegrationService
{
    public async Task NotifyEventAsync(string eventType, string payload, CancellationToken ct)
    {
        // In real life, this should be a message queue and URL should be configurable
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        content.Headers.Add("X-Event-Type", eventType);

        var response = await httpClient.PostAsync("https://api.erp.example.com/wms-webhooks", content, ct);

        // If returns 500/503 - will throw exception, which Polly will catch
        response.EnsureSuccessStatusCode();
    }
}
