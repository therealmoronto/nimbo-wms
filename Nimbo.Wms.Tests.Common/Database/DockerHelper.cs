using System.Runtime.InteropServices;
using Docker.DotNet;

namespace Nimbo.Wms.Tests.Common.Database;

public static class DockerHelper
{
    public static async Task<bool> IsDockerAvailableAsync()
    {
        try
        {
            var uri = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new Uri("npipe://./pipe/docker_engine")
                : new Uri("unix:///var/run/docker.sock");

            using var client = new DockerClientConfiguration(uri).CreateClient();
            await client.System.PingAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
