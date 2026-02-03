using Testcontainers.PostgreSql;

namespace Nimbo.Wms.Infrastructure.Tests.Infrastructure;

// ReSharper disable once ClassNeverInstantiated.Global
public class PostgresFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _container;

    public bool IsStarted => _container is not null;

    public string ConnectionString => _container?.GetConnectionString() ?? throw new InvalidOperationException("Container not initialized.");
    
    public async Task InitializeAsync()
    {
        if (!await DockerHelper.IsDockerAvailableAsync())
            return;

        _container = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("nimbo_wms")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
            await _container.StopAsync();
    }
}
