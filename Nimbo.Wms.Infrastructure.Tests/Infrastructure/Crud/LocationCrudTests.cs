using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Infrastructure.Tests.Infrastructure.Crud;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class LocationCrudTests
{
    private readonly PostgresFixture _fixture;

    public LocationCrudTests(PostgresFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Location_crud_should_work_successfully_test()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();

        var warehouseId = WarehouseId.From(Guid.NewGuid());
        var zoneId = ZoneId.From(Guid.NewGuid());
        var locationId = LocationId.From(Guid.NewGuid());

        var warehouse = new Warehouse(
            warehouseId,
            code: "WH-LOC",
            name: "Warehouse for locations",
            address: "Test address");

        var zone = new Zone(
            zoneId,
            warehouseId,
            code: "ZONE-L",
            name: "Storage Zone",
            type: ZoneType.Storage);

        var location = new Location(
            id: locationId,
            warehouseId: warehouseId,
            zoneId: zoneId,
            code: "A-01-01",
            position: "LOC-A-01-01",
            type: LocationType.Shelf);

        // Create
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Set<Warehouse>().Add(warehouse);
            db.Set<Zone>().Add(zone);
            db.Set<Location>().Add(location);

            await db.SaveChangesAsync();
        }

        // Read
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Location>()
                .SingleAsync(x => x.Id.Equals(locationId));

            loaded.WarehouseId.Should().Be(warehouseId);
            loaded.ZoneId.Should().Be(zoneId);
            loaded.Code.Should().Be("A-01-01");
            loaded.Position.Should().Be("LOC-A-01-01");
            loaded.Type.Should().Be(LocationType.Shelf);
        }

        // Update
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Location>()
                .SingleAsync(x => x.Id.Equals(locationId));

            loaded.ChangeType(LocationType.Picking);
            loaded.SetAddressParts(null, null, null, position: "LOC-A-01-01-UPD");

            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Location>()
                .SingleAsync(x => x.Id.Equals(locationId));

            loaded.Type.Should().Be(LocationType.Picking);
            loaded.Position.Should().Be("LOC-A-01-01-UPD");
        }

        // Delete
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Location>()
                .SingleAsync(x => x.Id.Equals(locationId));

            db.Remove(loaded);
            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var exists = await db.Set<Location>()
                .AnyAsync(x => x.Id.Equals(locationId));

            exists.Should().BeFalse();
        }
    }
}
