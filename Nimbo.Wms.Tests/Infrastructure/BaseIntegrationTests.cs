using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Infrastructure.DependencyInjection;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Tests.Common;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Tests.Infrastructure;

public abstract class BaseIntegrationTests : IClassFixture<PostgresFixture>
{
    public BaseIntegrationTests(PostgresFixture fixture)
    {
        Fixture = fixture;
        
        var services = new ServiceCollection();
        services.AddDbContext<NimboWmsDbContext>(options =>
        {
            options.UseNpgsql(fixture.ConnectionString);
        });

        services.AddInfrastructure();

        Services = services.BuildServiceProvider();
        Scope = Services.CreateScope();
    }

    protected PostgresFixture Fixture { get; }

    protected IServiceScope Scope { get; }

    protected IServiceProvider Services { get; }
    
    protected IUnitOfWork UnitOfWork =>
        Services.GetRequiredService<IUnitOfWork>();

    protected NimboWmsDbContext DbContext =>
        Services.GetRequiredService<NimboWmsDbContext>();
}
