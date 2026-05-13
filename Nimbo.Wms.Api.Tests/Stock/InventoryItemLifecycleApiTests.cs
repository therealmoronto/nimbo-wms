using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Contracts.Stock.Commands;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Models.MasterData;
using Nimbo.Wms.Models.Stock;
using Nimbo.Wms.Models.Topology;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests.Stock;

[IntegrationTest]
public class InventoryItemLifecycleApiTests : ApiTestBase
{
    public InventoryItemLifecycleApiTests(PostgresFixture postgres)
        : base(postgres) { }

    [Fact]
    public async Task CreateInventoryItem_GetById_ListInventoryItems_Succeeds()
    {
        // Setup: Create warehouse, location, and item
        var createWarehouseRequest = new CreateWarehouseCommand(
            Code: $"WH-{Guid.NewGuid():N}".Substring(0, 10),
            Name: "Test Warehouse");

        var createWarehouseResponse = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);
        var createdWarehouse = (await createWarehouseResponse.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseGuid = createdWarehouse.Id;

        var addZoneRequest = new AddZoneCommand(
            warehouseGuid,
            Code: "Z-TEST",
            Name: "Test Zone",
            nameof(ZoneType.Storage));

        var addZoneResponse = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/zones", addZoneRequest);
        var createdZone = (await addZoneResponse.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneGuid = createdZone.ZoneId;

        var addLocationRequest = new AddLocationCommand(
            warehouseGuid,
            zoneGuid,
            Code: "A-01-01-01",
            Type: nameof(LocationType.Shelf));

        var addLocationResponse = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/locations", addLocationRequest);
        var createdLocation = (await addLocationResponse.Content.ReadFromJsonAsync<AddLocationResponse>())!;
        var locationId = createdLocation.LocationId;

        var createItemRequest = new CreateItemCommand(
            "INV-TEST-ITEM",
            "ITI-001",
            "00101234",
            nameof(UnitOfMeasure.Kilogram));

        var createItemResponse = await Client.PostAsJsonAsync("/api/items", createItemRequest);
        var createdItem = (await createItemResponse.Content.ReadFromJsonAsync<CreateItemResponse>())!;
        var itemId = createdItem.ItemGuid;

        // 1) Create inventory item
        var createInventoryItemRequest = new CreateInventoryItemCommand(
            ItemId: itemId,
            WarehouseId: warehouseGuid,
            LocationId: locationId,
            Quantity: 100m,
            QuantityUom: nameof(UnitOfMeasure.Kilogram),
            Status: nameof(InventoryStatus.Available),
            BatchId: null,
            SerialNumber: null,
            UnitCost: 25.50m);

        var createInventoryItemResponse = await Client.PostAsJsonAsync("/api/stock/inventory-items", createInventoryItemRequest);

        createInventoryItemResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdInventoryItem = (await createInventoryItemResponse.Content.ReadFromJsonAsync<CreateInventoryItemResponse>())!;
        var inventoryItemId = createdInventoryItem.Id;

        // 2) Get inventory item by id
        var inventoryItemDto = await Client.GetFromJsonAsync<InventoryItemDto>($"/api/stock/inventory-items/{inventoryItemId}");

        inventoryItemDto.Should().NotBeNull();
        inventoryItemDto.Id.Should().Be(inventoryItemId);
        inventoryItemDto.ItemId.Should().Be(itemId);
        inventoryItemDto.WarehouseId.Should().Be(warehouseGuid);
        inventoryItemDto.LocationId.Should().Be(locationId);
        inventoryItemDto.Quantity.Value.Should().Be(100m);
        inventoryItemDto.Quantity.Uom.Should().Be(nameof(UnitOfMeasure.Kilogram));
        inventoryItemDto.Status.Should().Be(nameof(InventoryStatus.Available));
        inventoryItemDto.SerialNumber.Should().BeNull();
        inventoryItemDto.UnitCost.Should().Be(25.50m);

        // 3) List inventory items with filter
        var inventoryItems = await Client.GetFromJsonAsync<IReadOnlyList<InventoryItemDto>>($"/api/stock/inventory-items?warehouseGuid={warehouseGuid}&itemGuid={itemId}");

        inventoryItems.Should().NotBeNullOrEmpty();

        var listedInventoryItem = inventoryItems.Single(i => i.Id == inventoryItemId);
        listedInventoryItem.ItemId.Should().Be(itemId);
        listedInventoryItem.WarehouseId.Should().Be(warehouseGuid);
        listedInventoryItem.Quantity.Value.Should().Be(100m);
        listedInventoryItem.Status.Should().Be(nameof(InventoryStatus.Available));
    }
}
