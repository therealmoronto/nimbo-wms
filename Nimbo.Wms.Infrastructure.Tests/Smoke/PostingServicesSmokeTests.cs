using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Domain.Common;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Entities.Ledger;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;
using Nimbo.Wms.Infrastructure.Persistence;
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
        var (warehouseId, locationId, itemId, supplierId) = await SeedRequiredData();
        var receivingRepo = Scope.ServiceProvider.GetRequiredService<IReceivingDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<ReceivingDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var receivedQuantity = new Quantity(10, UnitOfMeasure.Piece);
        var expectedQuantity = new Quantity(10, UnitOfMeasure.Piece);
        var doc = new ReceivingDocument(ReceivingDocumentId.New(), warehouseId, supplierId, "REC-001", "REC", DateTime.UtcNow);
        doc.AddLine(itemId, receivedQuantity, locationId, expectedQuantity);
        doc.Start(); // Ensure status is InProgress

        await receivingRepo.AddAsync(doc);
        await uow.CommitAsync();

        // 2. Act
        await postingService.PostAsync(doc);
        await uow.CommitAsync();

        // 3. Assert Authoritative Stock — item is not batch-managed, so no prior lot exists at this
        // location; GetByCriteriaAsync(stockLotId: null) resolves unambiguously to the single new lot.
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var stock = await stockRepo.GetByCriteriaAsync(warehouseId, locationId, itemId, null);
        stock!.Quantity.Value.Should().Be(10);

        // 3b. Assert a StockLot was minted for this receipt even though the item isn't batch-managed —
        // this is what makes FIFO/FEFO possible for every item, not just batch-managed ones.
        var stockLotRepo = Scope.ServiceProvider.GetRequiredService<IStockLotRepository>();
        var stockLot = await stockLotRepo.GetByIdAsync(stock.StockLotId);
        stockLot.Should().NotBeNull();
        stockLot!.ReceivingDocumentId.Should().Be(doc.Id);
        stockLot.VendorLotId.Should().BeNull();

        // 4. Assert Ledger Traceability
        var ledgerRepo = Scope.ServiceProvider.GetRequiredService<IStockLedgerEntryRepository>();
        var entries = await ledgerRepo.GetByInventoryItemIdAsync(stock.Id);

        entries.Should().ContainSingle();
        var entry = entries.First();
        entry.TransactionType.Should().Be(LedgerTransactionType.Receipt);
        entry.QuantityDelta.Value.Should().Be(10);
        entry.BalanceAfter.Value.Should().Be(10);
        entry.SourceDocumentId.Should().Be(doc.Id.Value);
        entry.StockLotId.Should().Be(stock.StockLotId);
    }

    [Fact]
    public async Task ReceivingPost_BatchManagedItem_ReusesVendorLotAcrossReceipts()
    {
        // 1. Setup: item IS batch-managed
        var (warehouseId, locationId, itemId, supplierId) = await SeedRequiredData(isBatchManaged: true);
        var receivingRepo = Scope.ServiceProvider.GetRequiredService<IReceivingDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<ReceivingDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var expiryDate = DateTime.UtcNow.AddMonths(6);
        const string batchNumber = "LOT-2026-08";

        // Two receiving documents, same batch/expiry/supplier
        var doc1 = new ReceivingDocument(ReceivingDocumentId.New(), warehouseId, supplierId, "REC-101", "REC", DateTime.UtcNow);
        doc1.AddLine(itemId, new Quantity(5, UnitOfMeasure.Piece), locationId, new Quantity(5, UnitOfMeasure.Piece), expiryDate, batchNumber);
        doc1.Start();
        await receivingRepo.AddAsync(doc1);
        await uow.CommitAsync();
        await postingService.PostAsync(doc1);
        await uow.CommitAsync();

        var doc2 = new ReceivingDocument(ReceivingDocumentId.New(), warehouseId, supplierId, "REC-102", "REC", DateTime.UtcNow);
        doc2.AddLine(itemId, new Quantity(7, UnitOfMeasure.Piece), locationId, new Quantity(7, UnitOfMeasure.Piece), expiryDate, batchNumber);
        doc2.Start();
        await receivingRepo.AddAsync(doc2);
        await uow.CommitAsync();
        await postingService.PostAsync(doc2);
        await uow.CommitAsync();

        // Assert: two distinct StockLots (one per receipt), both linked to the SAME VendorLot — the
        // find-or-create-by-composite-key resolves to a single VendorLot for the shared batch/expiry/supplier.
        var dbContext = Scope.ServiceProvider.GetRequiredService<NimboWmsDbContext>();

        var stockLot1 = await dbContext.Set<StockLot>().SingleAsync(sl => sl.ReceivingDocumentId == doc1.Id);
        var stockLot2 = await dbContext.Set<StockLot>().SingleAsync(sl => sl.ReceivingDocumentId == doc2.Id);

        stockLot1.Id.Should().NotBe(stockLot2.Id);
        stockLot1.VendorLotId.Should().NotBeNull();
        stockLot1.VendorLotId.Should().Be(stockLot2.VendorLotId);

        var vendorLots = await dbContext.Set<VendorLot>().Where(v => v.ItemId == itemId).ToListAsync();
        vendorLots.Should().ContainSingle();
        vendorLots[0].BatchNumber.Should().Be(batchNumber);
    }

    [Fact]
    public async Task RelocationPost_ShouldPerformDoubleEntry()
    {
        // 1. Setup: Create initial stock at Source
        var (warehouseId, sourceLocId, itemId, _) = await SeedRequiredData();
        var targetLocId = await SeedLocation(warehouseId, "LOC-TARGET");

        var stockLotId = await SeedInitialStock(warehouseId, sourceLocId, itemId, 50);

        var relocationRepo = Scope.ServiceProvider.GetRequiredService<IRelocationDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<RelocationDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var moveQty = new Quantity(20, UnitOfMeasure.Piece);
        var doc = new RelocationDocument(RelocationDocumentId.New(), warehouseId, "MOV-001", "MOV", DateTime.UtcNow);
        doc.AddLine(itemId, moveQty, sourceLocId, targetLocId, stockLotId);
        doc.Start();

        await relocationRepo.AddAsync(doc);
        await uow.CommitAsync();

        // 2. Act
        await postingService.PostAsync(doc);
        await uow.CommitAsync();

        // 3. Assert Balances
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var sourceStock = await stockRepo.GetByCriteriaAsync(warehouseId, sourceLocId, itemId, stockLotId);
        var targetStock = await stockRepo.GetByCriteriaAsync(warehouseId, targetLocId, itemId, stockLotId);

        sourceStock!.Quantity.Value.Should().Be(30); // 50 - 20
        targetStock!.Quantity.Value.Should().Be(20); // 0 + 20

        // 3b. The target row inherits the source's StockLotId — physical stock keeps its lot identity
        // when relocated.
        targetStock.StockLotId.Should().Be(sourceStock.StockLotId);

        // 4. Assert Ledger Entries
        var ledgerRepo = Scope.ServiceProvider.GetRequiredService<IStockLedgerEntryRepository>();
        var sourceEntries = await ledgerRepo.GetByInventoryItemIdAsync(sourceStock.Id);
        var targetEntries = await ledgerRepo.GetByInventoryItemIdAsync(targetStock.Id);

        sourceEntries.Should().Contain(e => e.TransactionType == LedgerTransactionType.TransferOut && e.QuantityDelta == -20);
        targetEntries.Should().Contain(e => e.TransactionType == LedgerTransactionType.TransferIn && e.QuantityDelta == 20);
    }

    [Fact]
    public async Task CycleCountPost_ShouldCaptureDiscrepancy()
    {
        // 1. Setup: Seed initial stock (System thinks there are 10)
        var (warehouseId, locationId, itemId, _) = await SeedRequiredData();
        var stockLotId = await SeedInitialStock(warehouseId, locationId, itemId, 10);

        var cycleCountRepo = Scope.ServiceProvider.GetRequiredService<ICycleCountDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<CycleCountDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var doc = new CycleCountDocument(CycleCountDocumentId.New(), warehouseId, "CNT-001", "CNT", DateTime.UtcNow);
        var lineId = doc.AddLine(itemId, locationId, new Quantity(10, UnitOfMeasure.Piece), stockLotId); // BookQuantity = 10 internally
        var line = doc.GetLine(lineId);
        line.ChangeActualQuantity(new Quantity(12, UnitOfMeasure.Piece));
        doc.Complete(); // Move to Completed status so it can be Posted

        await cycleCountRepo.AddAsync(doc);
        await uow.CommitAsync();

        // 3. Act: Post the reconciliation
        await postingService.PostAsync(doc);
        await uow.CommitAsync();

        // 4. Assert Authoritative Stock is updated to the counted value (12)
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var stock = await stockRepo.GetByCriteriaAsync(warehouseId, locationId, itemId, stockLotId);
        stock!.Quantity.Value.Should().Be(12);

        // 5. Assert Ledger records only the discrepancy (+2)
        var ledgerRepo = Scope.ServiceProvider.GetRequiredService<IStockLedgerEntryRepository>();
        var entries = await ledgerRepo.GetByInventoryItemIdAsync(stock.Id);

        // We expect a Receipt (from Seed) and a CycleCount entry
        entries.Should().HaveCount(1);

        var countEntry = entries.First(e => e.TransactionType == LedgerTransactionType.CountingAdjustment);
        countEntry.QuantityDelta.Value.Should().Be(2);   // The discrepancy
        countEntry.BalanceAfter.Value.Should().Be(12); // The new authoritative total
        countEntry.SourceDocumentId.Should().Be(doc.Id.Value);
    }

    [Fact]
    public async Task CycleCountPost_SurplusWithNoPriorStockAndNoStockLotId_Throws()
    {
        // No prior InventoryItem row at this item/location, and no StockLotId given — surplus can't be
        // attributed to any lot, since a StockLot can only be minted from a ReceivingDocument.
        var (warehouseId, locationId, itemId, _) = await SeedRequiredData();

        var cycleCountRepo = Scope.ServiceProvider.GetRequiredService<ICycleCountDocumentRepository>();
        var postingService = Scope.ServiceProvider.GetRequiredService<IDocumentPostingService<CycleCountDocument>>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var doc = new CycleCountDocument(CycleCountDocumentId.New(), warehouseId, "CNT-901", "CNT", DateTime.UtcNow);
        var lineId = doc.AddLine(itemId, locationId, new Quantity(0, UnitOfMeasure.Piece)); // stockLotId: null
        var line = doc.GetLine(lineId);
        line.ChangeActualQuantity(new Quantity(5, UnitOfMeasure.Piece));
        doc.Complete();

        await cycleCountRepo.AddAsync(doc);
        await uow.CommitAsync();

        var act = async () => await postingService.PostAsync(doc);

        await act.Should().ThrowAsync<DomainException>();
    }

    private async Task<(WarehouseId WarehouseId, LocationId LocationId, ItemId ItemId, SupplierId supplierId)> SeedRequiredData(bool isBatchManaged = false)
    {
        await Fixture.EnsureMigratedAsync();

        var warehouseRepo = Scope.ServiceProvider.GetRequiredService<IWarehouseRepository>();
        var itemRepo = Scope.ServiceProvider.GetRequiredService<IItemRepository>();
        var supplierRepo = Scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        // 1. Create Warehouse
        var warehouse = new Warehouse(WarehouseId.New(), $"TST-WH{Guid.NewGuid().ToString()[..5]}", "Test Warehouse");

        // 2. Create Location within a Zone
        var zone = warehouse.AddZone(ZoneId.New(), $"TST-ZONE{Guid.NewGuid().ToString()[..5]}", "Zone 51", ZoneType.Storage);
        var location = warehouse.AddLocation(LocationId.New(), zone.Id, "LOC-01", LocationType.Pallet);

        await warehouseRepo.AddAsync(warehouse);

        // 3. Create Master Data Item
        var item = new Item(ItemId.New(), "ITM-001", Guid.NewGuid().ToString()[..5], "12345678", UnitOfMeasure.Piece, isBatchManaged);
        await itemRepo.AddAsync(item);

        var supplier = new Supplier(SupplierId.New(), $"SUP-{Guid.NewGuid().ToString()[..5]}", "Supplier #1", "Supplier Address");
        await supplierRepo.AddAsync(supplier);

        await uow.CommitAsync();

        return (warehouse.Id, location.Id, item.Id, supplier.Id);
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

    /// <summary>
    /// Seeds an InventoryItem with a real, persisted StockLot (backed by a real ReceivingDocument, to
    /// satisfy the FK) and returns the StockLotId, so tests exercising non-Receiving posting services
    /// have something valid to reference.
    /// </summary>
    private async Task<StockLotId> SeedInitialStock(WarehouseId whId, LocationId locId, ItemId itemId, decimal amount)
    {
        var receivingRepo = Scope.ServiceProvider.GetRequiredService<IReceivingDocumentRepository>();
        var stockLotRepo = Scope.ServiceProvider.GetRequiredService<IStockLotRepository>();
        var stockRepo = Scope.ServiceProvider.GetRequiredService<IInventoryItemRepository>();
        var uow = Scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var supplierRepo = Scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
        var supplier = new Supplier(SupplierId.New(), $"SUP-{Guid.NewGuid().ToString()[..5]}", "Seed Supplier", "Seed Address");
        await supplierRepo.AddAsync(supplier);

        var seedDoc = new ReceivingDocument(ReceivingDocumentId.New(), whId, supplier.Id, $"SEED-{Guid.NewGuid().ToString()[..5]}", "SEED", DateTime.UtcNow);
        await receivingRepo.AddAsync(seedDoc);

        var stockLot = new StockLot(StockLotId.New(), itemId, seedDoc.Id, DateTime.UtcNow);
        await stockLotRepo.AddAsync(stockLot);

        var stock = new InventoryItem(
            InventoryItemId.New(),
            itemId,
            whId,
            locId,
            stockLot.Id,
            new Quantity(amount, UnitOfMeasure.Piece));

        await stockRepo.AddAsync(stock);
        await uow.CommitAsync();

        return stockLot.Id;
    }
}
