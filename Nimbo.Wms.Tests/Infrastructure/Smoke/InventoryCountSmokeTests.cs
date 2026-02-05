using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Documents.Audit;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.WarehouseData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;
using Nimbo.Wms.Tests.Common;

namespace Nimbo.Wms.Tests.Infrastructure.Smoke;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class InventoryCountSmokeTests
{
    private readonly PostgresFixture _fixture;

    public InventoryCountSmokeTests(PostgresFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task InventoryCount_should_persist_and_restore_with_lines()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();

        // --- Arrange master data ---
        var warehouseId = WarehouseId.From(Guid.NewGuid());
        var zoneId = ZoneId.From(Guid.NewGuid());
        var locationId = LocationId.From(Guid.NewGuid());
        var itemId = ItemId.From(Guid.NewGuid());
        var inventoryCountId = InventoryCountId.From(Guid.NewGuid());

        var warehouse = new Warehouse(
            warehouseId,
            code: "WH-IC",
            name: "Inventory Count WH",
            address: "Test address");

        var zone = new Zone(
            zoneId,
            warehouseId,
            code: "ZONE-IC",
            name: "Count Zone",
            type: ZoneType.Storage);

        var location = new Location(
            id: locationId,
            warehouseId: warehouseId,
            zoneId: zoneId,
            code: "IC-LOC",
            position: "IC-LOC-01",
            type: LocationType.Shelf);

        var item = new Item(
            itemId,
            name: "Counted Item",
            internalSku: $"SKU-{Guid.NewGuid():N}".Substring(0, 32),
            barcode: null,
            baseUomCode: UnitOfMeasure.Piece,
            manufacturer: null,
            weightKg: null,
            volumeM3: null);

        // --- InventoryCount ---
        var inventoryCount = new InventoryCount(
            inventoryCountId,
            warehouseId,
            code: "IC-01",
            name: "Inventory Count 01",
            createdAt: DateTime.UtcNow);
        
        inventoryCount.Start(DateTime.UtcNow);

        var line = inventoryCount.AddLine(
            itemId: itemId,
            locationId: locationId,
            systemQuantity: new Quantity(10, UnitOfMeasure.Piece));
        
        line.SetCountedQuantity(new Quantity(8, UnitOfMeasure.Piece));

        line = inventoryCount.AddLine(
            itemId: itemId,
            locationId: locationId,
            systemQuantity: new Quantity(5, UnitOfMeasure.Piece));
        
        line.SetCountedQuantity(new Quantity(5, UnitOfMeasure.Piece));

        // --- Act: persist ---
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Add(warehouse);
            db.Add(zone);
            db.Add(location);
            db.Add(item);
            db.Add(inventoryCount);

            await db.SaveChangesAsync();
        }

        // --- Assert: restore ---
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<InventoryCount>()
                .Include(x => x.Lines)
                .SingleAsync(x => x.Id.Equals(inventoryCountId));

            loaded.WarehouseId.Should().Be(warehouseId);
            loaded.Code.Should().Be("IC-01");
            loaded.Name.Should().Be("Inventory Count 01");
            loaded.Status.Should().Be(InventoryCountStatus.InProgress);

            loaded.Lines.Should().HaveCount(2);

            loaded.Lines.Should().ContainSingle(l =>
                l.ItemId.Equals(itemId) &&
                l.LocationId.Equals(locationId) &&
                l.SystemQuantity == 10 &&
                l.CountedQuantity != null &&
                l.CountedQuantity == 8);

            loaded.Lines.Should().ContainSingle(l =>
                l.ItemId.Equals(itemId) &&
                l.LocationId.Equals(locationId) &&
                l.SystemQuantity == 5 &&
                l.CountedQuantity != null &&
                l.CountedQuantity == 5);
        }
    }
}
