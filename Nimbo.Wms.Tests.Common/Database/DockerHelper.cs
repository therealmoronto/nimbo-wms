using System.Runtime.InteropServices;
using Docker.DotNet;

namespace Nimbo.Wms.Tests.Common.Database;

public static class DockerHelper
{
    public static async Task<bool> IsDockerAvailableAsync()
    {
        var dockerHost = Environment.GetEnvironmentVariable("DOCKER_HOST");
        if (!string.IsNullOrEmpty(dockerHost))
        {
            try
            {
                using var client = new DockerClientConfiguration(new Uri(dockerHost)).CreateClient();
                await client.System.PingAsync();
                return true;
            }
            catch
            {
                // Fallback to default paths
            }
        }

        var defaultUris = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new[] { new Uri("npipe://./pipe/docker_engine") }
            : new[]
            {
                new Uri("unix:///var/run/docker.sock"),
                new Uri($"unix://{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".docker/run/docker.sock")}")
            };

        foreach (var uri in defaultUris)
        {
            try
            {
                using var client = new DockerClientConfiguration(uri).CreateClient();
                await client.System.PingAsync();
                return true;
            }
            catch
            {
                // Continue to next URI
            }
        }

        return false;
    }
}
