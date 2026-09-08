using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Models.MasterData;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests.MasterData;

[IntegrationTest]
public class ItemsLifecycleApiTests : ApiTestBase
{
    public ItemsLifecycleApiTests(PostgresFixture postgres)
        : base(postgres) { }

    [Fact]
    public async Task CreateItem_AndWholeLifecycle_Succeeds()
    {
        // 1) Create item
        var createItemRequest = new CreateItemCommand(
            "ITEM-001",
            "I-001",
            "00100245",
            nameof(UnitOfMeasure.Kilogram));

        var createResponse = await Client.PostAsJsonAsync("/api/items", createItemRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createItemResponse = (await createResponse.Content.ReadFromJsonAsync<CreateItemResponse>())!;
        var itemGuid = createItemResponse.Value;

        // 2) Get item by id
        var itemResponse = await Client.GetFromJsonAsync<GetItemResponse>($"/api/items/{itemGuid}");

        itemResponse.Should().NotBeNull();
        itemResponse.Value.Should().NotBeNull();

        var item = itemResponse.Value;

        item.Should().NotBeNull();
        item.Id.Should().Be(itemGuid);
        item.Name.Should().Be("ITEM-001");
        item.InternalSku.Should().Be("I-001");
        item.Barcode.Should().Be("00100245");
        item.BaseUomCode.Should().Be(nameof(UnitOfMeasure.Kilogram));
        
        // 3) Patch item
        var patchItemRequest = new PatchItemRequest(itemGuid)
        {
            Name = "ITEM-003",
            InternalSku = "I-003",
            Barcode = "00100147",
            BaseUom = nameof(UnitOfMeasure.Gram),
            Manufacturer = "MF-17",
            WeightKg = 1234.56m,
            VolumeM3 = 78.9m
        };

        var patchResponse = await Client.PatchAsJsonAsync($"/api/items/{itemGuid}", patchItemRequest);
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 4) Get list of items
        var itemsResponse = await Client.GetFromJsonAsync<GetItemsResponse>("/api/items");

        itemsResponse.Should().NotBeNull();
        itemsResponse.Value.Should().NotBeNullOrEmpty();

        var items = itemsResponse.Value;

        var updated = items.Single(i => i.Id == itemGuid);
        updated.Id.Should().Be(itemGuid);
        updated.Name.Should().Be("ITEM-003");
        updated.InternalSku.Should().Be("I-003");
        updated.Barcode.Should().Be("00100147");
        updated.BaseUomCode.Should().Be(nameof(UnitOfMeasure.Gram));
        updated.Manufacturer.Should().Be("MF-17");
        updated.WeightKg.Should().Be(1234.56m);
        updated.VolumeM3.Should().Be(78.9m);
        
        // 5) Delete item
        var deleteResponse = await Client.DeleteAsync($"/api/items/{itemGuid}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 6) Get list of items
        itemsResponse = await Client.GetFromJsonAsync<GetItemsResponse>("/api/items");
        itemsResponse.Should().NotBeNull();

        items = itemsResponse.Value;

        items.Should().NotBeNull();
        items.Should().NotContain(i => i.Id == itemGuid);
    }

    [Fact]
    private async Task DeleteItem_Returns404_WhenItemDoesNotExist()
    {
        var itemId = Guid.NewGuid();
        var response = await Client.DeleteAsync($"/api/items/{itemId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
