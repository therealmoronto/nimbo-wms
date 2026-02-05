using FluentAssertions;
using Nimbo.Wms.Tests.Common;

namespace Nimbo.Wms.Tests.Infrastructure.Smoke;

[IntegrationTest]
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
        
        await _fixture.EnsureMigratedAsync();
        
        var canConnect = await db.Database.CanConnectAsync();

        canConnect.Should().BeTrue();
    }
}
