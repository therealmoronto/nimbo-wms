using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        builder.UseTestServer();
        builder.UseSetting("ASPNETCORE_URLS", string.Empty);

        builder.ConfigureServices(services =>
        {
            // 1. Clearing EF Core from pool that were added by Aspire
            services.RemoveAll(typeof(DbContextOptions<NimboWmsDbContext>));

            // NOTE It's not safe so FIXME
            services.RemoveAll(typeof(IDbContextPool<NimboWmsDbContext>));
            services.RemoveAll(typeof(IScopedDbContextLease<NimboWmsDbContext>));

            // 2. Register back via pool to repeat real life behavior of Program.cs
            services.AddDbContextPool<NimboWmsDbContext>(options =>
            {
                options.UseNpgsql(
                    _postgres.ConnectionString,
                    npgsql => npgsql.MigrationsAssembly("Nimbo.Wms.Infrastructure"));
            });
        });
    }
}
