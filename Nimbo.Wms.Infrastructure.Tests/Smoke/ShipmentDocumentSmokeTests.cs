using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Infrastructure.Tests.Smoke;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class ShipmentDocumentSmokeTests : BaseIntegrationTests
{
    public ShipmentDocumentSmokeTests(PostgresFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task Shipment_document_should_persist_with_lines_and_pick_lines()
    {
        await Fixture.EnsureMigratedAsync();
        await using var ctx = DbContextFactory.Create(Fixture.ConnectionString);

        var warehouseId = WarehouseId.New();
        var warehouse = new Warehouse(warehouseId, "WH-001", "Warehouse #1", "Test address");
        var zone = new Zone(ZoneId.New(), warehouseId, "ZONE-001", "Zone #1", ZoneType.Storage);
        var location = new Location(LocationId.New(), warehouseId, zone.Id, "LOC-001", LocationType.Floor);
        var item = new Item(ItemId.New(), "ITEM-001", "Test Item", "SKU-001", UnitOfMeasure.Piece);

        ctx.Add(warehouse);
        ctx.Add(zone);
        ctx.Add(location);
        ctx.Add(item);

        await ctx.SaveChangesAsync();
        ctx.ChangeTracker.Clear();

        var createdAt = DateTime.UtcNow;

        var doc = new ShipmentDocument(
            ShipmentDocumentId.New(),
            warehouseId,
            "SHP-001",
            "Shipment 001",
            createdAt);

        doc.AddRequestedLine(item.Id, new Quantity(100m, UnitOfMeasure.Piece));
        
        doc.AddPickLine(item.Id, location.Id, new Quantity(40m, UnitOfMeasure.Piece));
        doc.AddPickLine(item.Id, location.Id, new Quantity(10m, UnitOfMeasure.Piece));

        ctx.Add(doc);
        await ctx.SaveChangesAsync();
        
        ctx.ChangeTracker.Clear();

        var loaded = await ctx.Set<ShipmentDocument>()
            .Include(x => x.Lines)
            .Include(x => x.PickLines)
            .SingleAsync(x => x.Id == doc.Id);

        doc.Id.Should().Be(loaded.Id);
        doc.WarehouseId.Should().Be(loaded.WarehouseId);
        doc.Code.Should().Be(loaded.Code);
        doc.Title.Should().Be(loaded.Title);
        doc.CreatedAt.Should().Be(loaded.CreatedAt);

        loaded.Lines.Should().NotBeNullOrEmpty();
        loaded.Lines.Should().ContainSingle(l => l.ItemId == item.Id);
        loaded.Lines.Should().ContainSingle(l => l.Quantity == 100m);

        loaded.PickLines.Should().NotBeNullOrEmpty();
        loaded.PickLines.Should().HaveCount(2);
        loaded.PickLines.Should().AllSatisfy(l =>
        {
            l.ItemId.Should().Be(item.Id);
            l.FromLocation.Should().Be(location.Id);
            l.Quantity.Value.Should().BeGreaterThan(0m);
        });

        loaded.PickLines.Sum(x => x.Quantity.Value).Should().Be(50m);
    }
}
