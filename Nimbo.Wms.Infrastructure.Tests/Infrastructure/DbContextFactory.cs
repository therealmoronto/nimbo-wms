using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.Tests.Infrastructure;

public static class DbContextFactory
{
    public static NimboWmsDbContext Create(string connectionString)
    {
        var options = new DbContextOptionsBuilder<NimboWmsDbContext>()
            .UseNpgsql(connectionString)
            .EnableSensitiveDataLogging()
            .Options;

        return new NimboWmsDbContext(options);
    }
}
