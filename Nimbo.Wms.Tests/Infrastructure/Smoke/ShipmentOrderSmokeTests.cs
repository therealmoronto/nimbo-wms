using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Documents.Outbound;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common;

namespace Nimbo.Wms.Tests.Infrastructure.Smoke;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class ShipmentOrderSmokeTests
{
    private readonly PostgresFixture _fixture;

    public ShipmentOrderSmokeTests(PostgresFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task ShipmentOrder_should_persist_and_restore_with_lines()
    {
        TestSkip.If(!_fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await _fixture.EnsureMigratedAsync();

        // --- Arrange master data ---
        var warehouseId = WarehouseId.From(Guid.NewGuid());
        var itemId = ItemId.From(Guid.NewGuid());

        var warehouse = new Warehouse(
            warehouseId,
            code: "WH-SO",
            name: "Shipment WH",
            address: "Test address");

        var item = new Item(
            itemId,
            name: "Ship Item",
            internalSku: $"SKU-{Guid.NewGuid():N}".Substring(0, 32),
            barcode: null,
            baseUomCode: UnitOfMeasure.Piece,
            manufacturer: null,
            weightKg: null,
            volumeM3: null);

        // --- ShipmentOrder ---
        var orderId = ShipmentOrderId.From(Guid.NewGuid());
        var customerId = CustomerId.From(Guid.NewGuid());

        var order = new ShipmentOrder(
            orderId,
            code: "SO-01",
            name: "Shipment Order 01",
            createdAt: DateTime.UtcNow,
            warehouseId: warehouseId,
            customerId: customerId);

        var line1 = order.AddLine(itemId, orderedQty: 10m, uomCode: UnitOfMeasure.Piece);
        var line2 = order.AddLine(itemId, orderedQty: 5m, uomCode: UnitOfMeasure.Piece);

        // --- Act: persist ---
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.Add(warehouse);
            db.Add(item);
            db.Add(order);

            await db.SaveChangesAsync();
        }

        // --- Assert: restore ---
        await using (var db = DbContextFactory.Create(_fixture.ConnectionString))
        {
            db.ChangeTracker.Clear();

            var loaded = await db.Set<ShipmentOrder>()
                .Include(x => x.Lines)
                .SingleAsync(x => x.Id.Equals(orderId));

            loaded.WarehouseId.Should().Be(warehouseId);
            loaded.CustomerId.Should().Be(customerId);

            loaded.Code.Should().Be("SO-01");
            loaded.Name.Should().Be("Shipment Order 01");
            loaded.Status.Should().Be(ShipmentOrderStatus.Draft);

            loaded.ShippedAt.Should().BeNull();
            loaded.CancelledAt.Should().BeNull();
            loaded.CancelReason.Should().BeNull();

            loaded.Lines.Should().HaveCount(2);

            loaded.Lines.Should().ContainSingle(l =>
                l.Id == line1.Id &&
                l.DocumentId.Equals(orderId) &&
                l.ItemId.Equals(itemId) &&
                l.OrderedQuantity == 10m &&
                l.ReservedQuantity == 0m &&
                l.PickedQuantity == 0m &&
                l.UomCode == UnitOfMeasure.Piece);

            loaded.Lines.Should().ContainSingle(l =>
                l.Id == line2.Id &&
                l.DocumentId.Equals(orderId) &&
                l.ItemId.Equals(itemId) &&
                l.OrderedQuantity == 5m &&
                l.ReservedQuantity == 0m &&
                l.PickedQuantity == 0m &&
                l.UomCode == UnitOfMeasure.Piece);
        }
    }
}
