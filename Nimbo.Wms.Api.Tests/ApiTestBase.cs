using System.Net.Http.Headers;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests;

public abstract class ApiTestBase: IAsyncLifetime
{
    private readonly PostgresFixture _postgres;

    protected ApiTestBase(PostgresFixture postgres)
    {
        _postgres = postgres;
    }

    protected NimboWmsApiFactory Factory { get; private set; } = null!;

    protected HttpClient Client { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _postgres.EnsureMigratedAsync();

        Factory = new NimboWmsApiFactory(_postgres);
        Client = Factory.CreateClient();

        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        Factory.Dispose();
        return Task.CompletedTask;
    }
}
