using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Nimbo.Wms.Tests.Infrastructure;

// ReSharper disable once ClassNeverInstantiated.Global
public class PostgresFixture : IAsyncLifetime
{
    private readonly SemaphoreSlim _migrateLock = new(1, 1);
    private bool _isMigrated;
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

    public async Task EnsureMigratedAsync()
    {
        if (_isMigrated)
        {
            return;
        }

        await _migrateLock.WaitAsync();
        try
        {
            if (_isMigrated)
                return;

            await using var db = DbContextFactory.Create(ConnectionString);
            await db.Database.MigrateAsync();

            _isMigrated = true;
        }
        finally
        {
            _migrateLock.Release();
        }
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
            await _container.StopAsync();
    }
}
