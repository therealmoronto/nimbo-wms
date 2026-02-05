using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.WarehouseData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common;

namespace Nimbo.Wms.Tests.Infrastructure.Crud;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class ZoneCrudTests
{
    private readonly PostgresFixture _fixture;

    public ZoneCrudTests(PostgresFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Zone_crud_should_work_successfully_test()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();

        var warehouseId = WarehouseId.From(Guid.NewGuid());
        var zoneId = ZoneId.From(Guid.NewGuid());

        var warehouse = new Warehouse(
            warehouseId,
            code: "WH-ZONE",
            name: "Warehouse for zones",
            address: "Test address");

        var zone = new Zone(
            zoneId,
            warehouseId,
            code: "ZONE-A",
            name: "Picking Zone",
            type: ZoneType.Picking);

        // Create
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Set<Warehouse>().Add(warehouse);
            db.Set<Zone>().Add(zone);

            await db.SaveChangesAsync();
        }

        // Read
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Zone>()
                .SingleAsync(x => x.Id.Equals(zoneId));

            loaded.Code.Should().Be("ZONE-A");
            loaded.Name.Should().Be("Picking Zone");
            loaded.Type.Should().Be(ZoneType.Picking);
            loaded.WarehouseId.Should().Be(warehouseId);
        }

        // Update
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Zone>()
                .SingleAsync(x => x.Id.Equals(zoneId));

            loaded.Rename("Updated Zone");
            loaded.ChangeType(ZoneType.Storage);

            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Zone>()
                .SingleAsync(x => x.Id.Equals(zoneId));

            loaded.Name.Should().Be("Updated Zone");
            loaded.Type.Should().Be(ZoneType.Storage);
        }

        // Delete
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Zone>()
                .SingleAsync(x => x.Id.Equals(zoneId));

            db.Remove(loaded);
            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var exists = await db.Set<Zone>()
                .AnyAsync(x => x.Id.Equals(zoneId));

            exists.Should().BeFalse();
        }
    }
}
