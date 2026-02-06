using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common;

namespace Nimbo.Wms.Tests.Infrastructure.Smoke;

[IntegrationTest]
[Collection(PostgresCollection.Name)]
public class SupplierRepositoryTests : BaseIntegrationTests
{
    public SupplierRepositoryTests(PostgresFixture fixture)
        : base(fixture) { }

    private ISupplierRepository SupplierRepository => Services.GetRequiredService<ISupplierRepository>();

    private IItemRepository ItemRepository => Services.GetRequiredService<IItemRepository>();

    [Fact]
    public async Task Add_supplier_item_and_persist_changes()
    {
        TestSkip.If(!Fixture.IsStarted, "Docker is not available. Start Docker Engine to run integration tests locally.");

        await Fixture.EnsureMigratedAsync();

        var supplier = new Supplier(
            SupplierId.New(),
            code: "SUP-001",
            name: "Test Supplier"
        );

        var item = new Item(
            ItemId.New(),
            name: "Item for test",
            internalSku: "SKU-001",
            barcode: "BAR-001",
            baseUomCode: UnitOfMeasure.Piece);

        await ItemRepository.AddAsync(item);
        await SupplierRepository.AddAsync(supplier);
        await UnitOfWork.SaveChangesAsync();

        supplier.AddItem(
            SupplierItemId.New(),
            item.Id,
            supplierSku: "SKU-1",
            supplierBarcode: "BAR-1",
            defaultPurchasePrice: 10m,
            purchaseUomCode: "BOX",
            unitsPerPurchaseUom: 10,
            leadTimeDays: 5,
            minOrderQty: 1,
            isPreferred: true
        );

        await UnitOfWork.SaveChangesAsync();

        var reloaded = await SupplierRepository.GetByIdAsync(supplier.Id);

        Assert.NotNull(reloaded);
        Assert.NotNull(reloaded.Items);
        Assert.Single(reloaded.Items);
    }
}
