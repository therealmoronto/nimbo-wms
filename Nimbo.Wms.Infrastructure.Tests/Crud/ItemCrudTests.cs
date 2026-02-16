using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Infrastructure.Tests.Crud;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class ItemCrudTests
{
    private readonly PostgresFixture _fixture;

    public ItemCrudTests(PostgresFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task Item_crud_should_work_successfully_test()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();

        var guid = Guid.NewGuid();
        var id = ItemId.From(guid);

        var item = new Item(
            id,
            name: "Test Item",
            internalSku: $"SKU-{guid:N}".Substring(0, 32),
            barcode: "1234567890",
            baseUomCode: UnitOfMeasure.Piece,
            manufacturer: "ACME",
            weightKg: 1.5m,
            volumeM3: 0.01m);

        // Create
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Set<Item>().Add(item);
            await db.SaveChangesAsync();
        }

        // Read
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Item>()
                .SingleAsync(x => x.Id.Equals(id));

            loaded.Name.Should().Be("Test Item");
            loaded.InternalSku.Should().StartWith("SKU-");
            loaded.Barcode.Should().Be("1234567890");
            loaded.BaseUomCode.Should().Be(UnitOfMeasure.Piece);
            loaded.Manufacturer.Should().Be("ACME");
            loaded.WeightKg.Should().Be(1.5m);
            loaded.VolumeM3.Should().Be(0.01m);
        }

        // Update
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Item>()
                .SingleAsync(x => x.Id.Equals(id));

            loaded.Rename("Renamed Item");
            loaded.ChangeBaseUom(UnitOfMeasure.Kilogram);
            loaded.ChangeBarcode(null);
            loaded.ChangeManufacturer("New Manufacturer");
            loaded.SetPhysical(2.0m, 0.02m);

            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<Item>()
                .SingleAsync(x => x.Id.Equals(id));

            loaded.Name.Should().Be("Renamed Item");
            loaded.BaseUomCode.Should().Be(UnitOfMeasure.Kilogram);
            loaded.Barcode.Should().BeNull();
            loaded.Manufacturer.Should().Be("New Manufacturer");
            loaded.WeightKg.Should().Be(2.0m);
            loaded.VolumeM3.Should().Be(0.02m);
        }

        // Delete
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var loaded = await db.Set<Item>()
                .SingleAsync(x => x.Id.Equals(id));

            db.Remove(loaded);
            await db.SaveChangesAsync();
        }

        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            var exists = await db.Set<Item>().AnyAsync(x => x.Id.Equals(id));
            exists.Should().BeFalse();
        }
    }
}
