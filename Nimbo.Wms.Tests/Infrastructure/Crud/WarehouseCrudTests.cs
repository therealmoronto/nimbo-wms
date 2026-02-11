using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Tests.Common;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Tests.Infrastructure.Crud;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class WarehouseCrudTests
{
    private readonly PostgresFixture _fixture;

    public WarehouseCrudTests(PostgresFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Warehouse_crud_should_work_successfully_test()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();
        
        var guid = Guid.NewGuid();
        var id = WarehouseId.From(guid);
        var code = $"W-{guid}".Substring(0, 32);
        
        var warehouse = new Warehouse(id, code, "Test Warehouse");

        // Create
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Set<Warehouse>().Add(warehouse);
            await db.SaveChangesAsync();
        }
        
        // Read
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();
            
            var loaded = await db.Set<Warehouse>()
                .SingleAsync(x => x.Id.Equals(id));

            loaded.Code.Should().Be(code);
            loaded.Name.Should().Be("Test Warehouse");
            loaded.Address.Should().BeNull();
            loaded.Description.Should().BeNull();
            loaded.IsActive.Should().BeTrue();
        }
        
        // Update
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Warehouse>()
                .SingleAsync(x => x.Id.Equals(id));
            
            loaded.Rename("Renamed Warehouse");
            loaded.Deactivate();
            
            await db.SaveChangesAsync();
        }
        
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();
            
            var loaded = await db.Set<Warehouse>()
                .SingleAsync(x => x.Id.Equals(id));
            
            loaded.Name.Should().Be("Renamed Warehouse");
            loaded.IsActive.Should().BeFalse();
        }
        
        // Delete
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Warehouse>()
                .SingleAsync(x => x.Id.Equals(id));
            
            db.Remove(loaded);
            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var exists = await db.Set<Warehouse>().AnyAsync(x => x.Id.Equals(id));
            exists.Should().BeFalse();
        }
    }
}
