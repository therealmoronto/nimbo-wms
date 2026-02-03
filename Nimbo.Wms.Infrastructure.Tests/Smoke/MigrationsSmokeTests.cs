using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Infrastructure.Tests.Infrastructure;

namespace Nimbo.Wms.Infrastructure.Tests.Smoke;

[Collection(PostgresCollection.Name)]
public class MigrationsSmokeTests
{
    private readonly PostgresFixture _fixture;
    
    public MigrationsSmokeTests(PostgresFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Database_migrations_should_apply_successfully()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await using var db = DbContextFactory.Create(_fixture.ConnectionString);
        
        await db.Database.MigrateAsync();
        
        var canConnect = await db.Database.CanConnectAsync();

        canConnect.Should().BeTrue();
    }
}
