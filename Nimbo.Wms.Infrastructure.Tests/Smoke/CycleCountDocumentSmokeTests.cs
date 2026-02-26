using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
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
public class CycleCountDocumentSmokeTests : BaseIntegrationTests
{
    public CycleCountDocumentSmokeTests(PostgresFixture fixture)
        : base(fixture)
    {
    }

    [Fact]
    public async Task CycleCount_document_should_persist_with_lines()
    {
        await Fixture.EnsureMigratedAsync();
        await using var ctx = DbContextFactory.Create(Fixture.ConnectionString);

        var warehouse = new Warehouse(WarehouseId.New(), "WH-002", "Warehouse #1", "Test address");
        var zone = new Zone(ZoneId.New(), warehouse.Id, "ZONE-002", "Zone #1", ZoneType.Storage);
        var location = new Location(LocationId.New(), warehouse.Id, zone.Id, "LOC-001", LocationType.Floor);
        var item = new Item(ItemId.New(), "ITEM-002", "Test Item", "SKU-002", UnitOfMeasure.Piece);

        ctx.Add(warehouse);
        ctx.Add(zone);
        ctx.Add(location);
        ctx.Add(item);
        await ctx.SaveChangesAsync();

        ctx.ChangeTracker.Clear();

        var doc = new CycleCountDocument(
            CycleCountDocumentId.New(),
            warehouse.Id,
            "CNT-001",
            "Cycle Count 001",
            DateTime.UtcNow);

        var guid = doc.AddLine(item.Id, location.Id, new Quantity(100m, UnitOfMeasure.Piece));
        doc.ChangeLineActualQuantity(guid, new Quantity(50m, UnitOfMeasure.Piece));

        ctx.Add(doc);
        await ctx.SaveChangesAsync();

        ctx.ChangeTracker.Clear();

        var loaded = await ctx.Set<CycleCountDocument>()
            .Include(x => x.Lines)
            .SingleAsync(x => x.Id == doc.Id);

        doc.Id.Should().Be(loaded.Id);
        doc.WarehouseId.Should().Be(loaded.WarehouseId);
        doc.Code.Should().Be(loaded.Code);
        doc.CreatedAt.Should().BeCloseTo(loaded.CreatedAt, TimeSpan.FromMilliseconds(1));

        loaded.Lines.Should().NotBeNullOrEmpty();
        loaded.Lines.Should().ContainSingle(l => l.ItemId == item.Id);
        loaded.Lines.Should().ContainSingle(l => l.Quantity == 100m);
        loaded.Lines.Should().ContainSingle(l => l.ActualQuantity != null && l.ActualQuantity.Value == 50m);
    }
}
