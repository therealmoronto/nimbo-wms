using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Infrastructure.Tests.Smoke;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class PostingServicesSmokeTests : BaseIntegrationTests
{
    public PostingServicesSmokeTests(PostgresFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task ReceivingPost_ShouldUpdateStockAndCreateLedger()
    {
        // 1. Setup Master Data & Document
        var (warehouseId, locationId, itemId) = await SeedRequiredData();
        var receivingRepo = Scope.ServiceProvider.GetRequiredService<IReceivingDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<ReceivingDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var receivedQuantity = new Quantity(10, UnitOfMeasure.Piece);
        var expectedQuantity = new Quantity(10, UnitOfMeasure.Piece);
        var doc = new ReceivingDocument(ReceivingDocumentId.New(), warehouseId, "REC-001", "REC", DateTime.UtcNow);
        doc.AddLine(itemId, receivedQuantity, locationId, expectedQuantity, null);
        doc.Start(); // Ensure status is InProgress

        await receivingRepo.AddAsync(doc);
        await uow.CommitAsync();

        // 2. Act
        await postingService.PostAsync(doc);
        await uow.CommitAsync();

        // 3. Assert Authoritative Stock
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var stock = await stockRepo.GetByCriteriaAsync(warehouseId, locationId, itemId);
        stock!.Quantity.Value.Should().Be(10);

        // 4. Assert Ledger Traceability
        var ledgerRepo = Scope.ServiceProvider.GetRequiredService<IStockLedgerEntryRepository>();
        var entries = await ledgerRepo.GetByInventoryItemIdAsync(stock.Id);

        entries.Should().ContainSingle();
        var entry = entries.First();
        entry.TransactionType.Should().Be(LedgerTransactionType.Receipt);
        entry.QuantityDelta.Should().Be(10);
        entry.BalanceAfter.Should().Be(10);
        entry.SourceDocumentId.Should().Be(doc.Id.Value);
    }

    [Fact]
    public async Task RelocationPost_ShouldPerformDoubleEntry()
    {
        // 1. Setup: Create initial stock at Source
        var (warehouseId, sourceLocId, itemId) = await SeedRequiredData();
        var targetLocId = await SeedLocation(warehouseId, "LOC-TARGET");

        await SeedInitialStock(warehouseId, sourceLocId, itemId, 50);

        var relocationRepo = Scope.ServiceProvider.GetRequiredService<IRelocationDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<RelocationDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var moveQty = new Quantity(20, UnitOfMeasure.Piece);
        var doc = new RelocationDocument(RelocationDocumentId.New(), warehouseId, "MOV-001", "MOV", DateTime.UtcNow);
        doc.AddLine(itemId, moveQty, sourceLocId, targetLocId);
        doc.Start();

        await relocationRepo.AddAsync(doc);
        await uow.CommitAsync();

        // 2. Act
        await postingService.PostAsync(doc);
        await uow.CommitAsync();

        // 3. Assert Balances
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var sourceStock = await stockRepo.GetByCriteriaAsync(warehouseId, sourceLocId, itemId);
        var targetStock = await stockRepo.GetByCriteriaAsync(warehouseId, targetLocId, itemId);

        sourceStock!.Quantity.Value.Should().Be(30); // 50 - 20
        targetStock!.Quantity.Value.Should().Be(20); // 0 + 20

        // 4. Assert Ledger Entries
        var ledgerRepo = Scope.ServiceProvider.GetRequiredService<IStockLedgerEntryRepository>();
        var sourceEntries = await ledgerRepo.GetByInventoryItemIdAsync(sourceStock.Id);
        var targetEntries = await ledgerRepo.GetByInventoryItemIdAsync(targetStock.Id);

        sourceEntries.Should().Contain(e => e.TransactionType == LedgerTransactionType.TransferOut && e.QuantityDelta == -20);
        targetEntries.Should().Contain(e => e.TransactionType == LedgerTransactionType.TransferIn && e.QuantityDelta == 20);
    }

    private async Task<(WarehouseId WarehouseId, LocationId LocationId, ItemId ItemId)> SeedRequiredData()
    {
        await Fixture.EnsureMigratedAsync();

        var warehouseRepo = Scope.ServiceProvider.GetRequiredService<IWarehouseRepository>();
        var itemRepo = Scope.ServiceProvider.GetRequiredService<IItemRepository>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        // 1. Create Warehouse
        var warehouse = new Warehouse(WarehouseId.New(), "TST-WH", "Test Warehouse");

        // 2. Create Location within a Zone
        var zone = warehouse.AddZone(ZoneId.New(), "TST-ZONE", "Zone 51", ZoneType.Storage);
        var location = warehouse.AddLocation(LocationId.New(), zone.Id, "LOC-01", LocationType.Pallet);

        await warehouseRepo.AddAsync(warehouse);

        // 3. Create Master Data Item
        var item = new Item(ItemId.New(), "ITM-001", "Test Item", "12345678", UnitOfMeasure.Piece);
        await itemRepo.AddAsync(item);

        await uow.CommitAsync();

        return (warehouse.Id, location.Id, item.Id);
    }

    private async Task<LocationId> SeedLocation(WarehouseId warehouseId, string code)
    {
        var warehouseRepo = Scope.ServiceProvider.GetRequiredService<IWarehouseRepository>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var warehouse = await warehouseRepo.GetByIdAsync(warehouseId);
        var zone = warehouse!.Zones.First(); // Reuse existing zone
        var location = warehouse.AddLocation(LocationId.New(), zone.Id, code, LocationType.Pallet);

        await uow.CommitAsync();
        return location.Id;
    }

    private async Task SeedInitialStock(WarehouseId whId, LocationId locId, ItemId itemId, decimal amount)
    {
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var stock = new InventoryItem(
            InventoryItemId.New(),
            itemId,
            whId,
            locId,
            new Quantity(amount, UnitOfMeasure.Piece));

        await stockRepo.AddAsync(stock);
        await uow.CommitAsync();
    }
}
