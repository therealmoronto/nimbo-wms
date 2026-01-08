using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Nimbo.Wms.Infrastructure.Persistence;

[PublicAPI]
public sealed class NimboWmsDbContextFactory : IDesignTimeDbContextFactory<NimboWmsDbContext>
{
    public NimboWmsDbContext CreateDbContext(string[] args)
    {
        // 1) Prefer env var (works in CI too)
        var connectionString =
            Environment.GetEnvironmentVariable("NimboWmsDb")
            ?? "Host=localhost;Port=5432;Database=nimbo_wms;Username=nimbo_admin;Password=im1i0pgf";

        var optionsBuilder = new DbContextOptionsBuilder<NimboWmsDbContext>()
            .UseNpgsql(connectionString, npgsql =>
            {
                // migrations stay in Infrastructure
                npgsql.MigrationsAssembly(typeof(NimboWmsDbContext).Assembly.FullName);
            });

        return new NimboWmsDbContext(optionsBuilder.Options);
    }
}
