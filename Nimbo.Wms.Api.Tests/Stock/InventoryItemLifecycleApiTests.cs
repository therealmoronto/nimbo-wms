using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Http;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Domain.ValueObject;
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
        var createWarehouseRequest = new CreateWarehouseRequest(
            Code: $"WH-{Guid.NewGuid():N}".Substring(0, 10),
            Name: "Test Warehouse");

        var createWarehouseResponse = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);
        var createdWarehouse = (await createWarehouseResponse.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = createdWarehouse.Id;

        var addZoneRequest = new AddZoneRequest(
            Code: "Z-TEST",
            Name: "Test Zone",
            Type: ZoneType.Storage);

        var addZoneResponse = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseId}/zones", addZoneRequest);
        var createdZone = (await addZoneResponse.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneId = createdZone.ZoneId;

        var addLocationRequest = new AddLocationRequest(
            ZoneId: zoneId,
            Code: "A-01-01-01",
            Type: LocationType.Shelf);

        var addLocationResponse = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseId}/locations", addLocationRequest);
        var createdLocation = (await addLocationResponse.Content.ReadFromJsonAsync<AddLocationResponse>())!;
        var locationId = createdLocation.LocationId;

        var createItemRequest = new CreateItemRequest(
            "INV-TEST-ITEM",
            "ITI-001",
            "00101234",
            UnitOfMeasure.Kilogram);

        var createItemResponse = await Client.PostAsJsonAsync("/api/items", createItemRequest);
        var createdItem = (await createItemResponse.Content.ReadFromJsonAsync<CreateItemResponse>())!;
        var itemId = createdItem.Id;

        // 1) Create inventory item
        var createInventoryItemRequest = new CreateInventoryItemRequest(
            ItemId: itemId,
            WarehouseId: warehouseId,
            LocationId: locationId,
            Quantity: new Quantity(100m, UnitOfMeasure.Kilogram),
            Status: InventoryStatus.Available,
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
        inventoryItemDto.WarehouseId.Should().Be(warehouseId);
        inventoryItemDto.LocationId.Should().Be(locationId);
        inventoryItemDto.Quantity.Value.Should().Be(100m);
        inventoryItemDto.Quantity.Uom.Should().Be(UnitOfMeasure.Kilogram);
        inventoryItemDto.Status.Should().Be(InventoryStatus.Available);
        inventoryItemDto.SerialNumber.Should().BeNull();
        inventoryItemDto.UnitCost.Should().Be(25.50m);

        // 3) List inventory items with filter
        var inventoryItems = await Client.GetFromJsonAsync<IReadOnlyList<InventoryItemDto>>($"/api/stock/inventory-items?warehouseGuid={warehouseId}&itemGuid={itemId}");

        inventoryItems.Should().NotBeNullOrEmpty();

        var listedInventoryItem = inventoryItems.Single(i => i.Id == inventoryItemId);
        listedInventoryItem.ItemId.Should().Be(itemId);
        listedInventoryItem.WarehouseId.Should().Be(warehouseId);
        listedInventoryItem.Quantity.Value.Should().Be(100m);
        listedInventoryItem.Status.Should().Be(InventoryStatus.Available);
    }
}
