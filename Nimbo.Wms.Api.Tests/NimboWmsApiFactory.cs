using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests;

public class NimboWmsApiFactory : WebApplicationFactory<Program>
{
    private readonly PostgresFixture _postgres;

    public NimboWmsApiFactory(PostgresFixture postgres)
    {
        _postgres = postgres;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Force in-memory hosting for tests to avoid binding to a real TCP port (e.g., 5281)
        builder.UseTestServer();

        // Ensure we don't inherit a fixed port from launchSettings/ASPNETCORE_URLS
        builder.UseSetting("ASPNETCORE_URLS", string.Empty);

        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<NimboWmsDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            // Re-register DbContext with container connection string
            services.AddDbContext<NimboWmsDbContext>(options =>
            {
                options.UseNpgsql(
                    _postgres.ConnectionString,
                    npgsql => npgsql.MigrationsAssembly("Nimbo.Wms.Infrastructure"));
            });
        });
    }
}
