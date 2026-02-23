using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.References;
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
        var createItemRequest = new CreateItemRequest(
            "ITEM-001", "I-001",
            "00100245",
            UnitOfMeasure.Kilogram);

        var createResponse = await Client.PostAsJsonAsync("/api/items", createItemRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var createItemResponse = (await createResponse.Content.ReadFromJsonAsync<CreateItemResponse>())!;
        var itemId = createItemResponse.Id;

        // 2) Get item by id
        var created = await Client.GetFromJsonAsync<ItemDto>($"/api/items/{itemId}");

        created.Should().NotBeNull();
        created.Id.Should().Be(itemId);
        created.Name.Should().Be("ITEM-001");
        created.InternalSku.Should().Be("I-001");
        created.Barcode.Should().Be("00100245");
        created.BaseUom.Should().Be(UnitOfMeasure.Kilogram);
        
        // 3) Patch item
        var patchItemRequest = new PatchItemRequest()
        {
            Name = "ITEM-003",
            InternalSku = "I-003",
            Barcode = "00100147",
            BaseUom = UnitOfMeasure.Gram,
            Manufacturer = "MF-17",
            WeightKg = 1234.56m,
            VolumeM3 = 78.9m
        };

        var patchResponse = await Client.PatchAsJsonAsync($"/api/items/{itemId}", patchItemRequest);
        patchResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 4) Get list of items
        var items = await Client.GetFromJsonAsync<List<ItemDto>>("/api/items");
        items.Should().NotBeNullOrEmpty();

        var updated = items.Single(i => i.Id == itemId);
        updated.Id.Should().Be(itemId);
        updated.Name.Should().Be("ITEM-003");
        updated.InternalSku.Should().Be("I-003");
        updated.Barcode.Should().Be("00100147");
        updated.BaseUom.Should().Be(UnitOfMeasure.Gram);
        updated.Manufacturer.Should().Be("MF-17");
        updated.WeightKg.Should().Be(1234.56m);
        updated.VolumeM3.Should().Be(78.9m);
        
        // 5) Delete item
        var deleteResponse = await Client.DeleteAsync($"/api/items/{itemId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        // 6) Get list of items
        items = await Client.GetFromJsonAsync<List<ItemDto>>("/api/items");
        items.Should().NotBeNull();
        items.Should().NotContain(i => i.Id == itemId);
    }

    [Fact]
    private async Task DeleteItem_Returns404_WhenItemDoesNotExist()
    {
        var itemId = Guid.NewGuid();
        var response = await Client.DeleteAsync($"/api/items/{itemId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
