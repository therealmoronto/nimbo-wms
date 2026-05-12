using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Models.MasterData;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests.MasterData;

[IntegrationTest]
public class SupplierLifecycleApiTests : ApiTestBase
{
    public SupplierLifecycleApiTests(PostgresFixture postgres)
        : base(postgres) { }

    [Fact]
    public async Task CreateSupplier_AndWholeLifecycle_Succeeds()
    {
        var createSupplierRequest = new CreateSupplierRequest(
            "SUP-001",
            "Supplier 1");
        
        var createResponse = await Client.PostAsJsonAsync("/api/suppliers", createSupplierRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createSupplierResponse = (await createResponse.Content.ReadFromJsonAsync<CreateSupplierResponse>())!;
        var supplierGuid = createSupplierResponse.SupplierGuid;
        var patchSupplierRequest = new PatchSupplierRequest(
            supplierGuid,
            "SUP-002",
            "Supplier 2",
            "1105506043",
            "Address lane 1, 0015",
            "John Doe",
            "655-doe",
            "john@doe.com",
            false);

        var createItemRequest = new CreateItemCommand(
            "SUPPLIER-ITEM-001",
            "SI-001",
            "00100245",
            nameof(UnitOfMeasure.Kilogram));
        
        var createItemResponse = await Client.PostAsJsonAsync("/api/items", createItemRequest);
        var createdItem = (await createItemResponse.Content.ReadFromJsonAsync<CreateItemResponse>())!;
        var itemGuid = createdItem.ItemGuid;

        var addSupplierItemRequest = new AddSupplierItemCommand(supplierGuid, itemGuid);
        var addedSupplierItemResponse = await Client.PostAsJsonAsync($"/api/suppliers/{supplierGuid}/items", addSupplierItemRequest);
        addedSupplierItemResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var addedSupplierItem = (await addedSupplierItemResponse.Content.ReadFromJsonAsync<AddSupplierItemResponse>())!;
        var supplierItemGuid = addedSupplierItem.SupplierItemGuid;

        var patchSupplierItemRequest = new PatchSupplierItemRequest(
            supplierGuid,
            supplierItemGuid,
            "SU-003",
            "001004532",
            100m,
            null,
            null,
            null,
            null,
            true);

        var patchSupplierItemResponse = await Client.PatchAsJsonAsync($"/api/suppliers/{supplierGuid}/items/{supplierItemGuid}", patchSupplierItemRequest);
        patchSupplierItemResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var patchResponse = await Client.PatchAsJsonAsync($"/api/suppliers/{supplierGuid}", patchSupplierRequest);

        patchResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var supplierDto = await Client.GetFromJsonAsync<SupplierDto>($"/api/suppliers/{supplierGuid}");

        supplierDto.Should().NotBeNull();
        supplierDto.Id.Should().Be(supplierGuid);
        supplierDto.Code.Should().Be("SUP-002");
        supplierDto.Name.Should().Be("Supplier 2");
        supplierDto.TaxId.Should().Be("1105506043");
        supplierDto.Address.Should().Be("Address lane 1, 0015");
        supplierDto.ContactName.Should().Be("John Doe");
        supplierDto.Phone.Should().Be("655-doe");
        supplierDto.Email.Should().Be("john@doe.com");
        supplierDto.IsActive.Should().BeFalse();

        supplierDto.Items.Should().NotBeNullOrEmpty();
        supplierDto.Items.Count.Should().Be(1);

        var supplierItemDto = supplierDto.Items.Single();
        supplierItemDto.Id.Should().Be(supplierItemGuid);
        supplierItemDto.SupplierId.Should().Be(supplierGuid);
        supplierItemDto.ItemId.Should().Be(itemGuid);
        supplierItemDto.DefaultPurchasePrice.Should().Be(100m);
        supplierItemDto.IsPreferred.Should().BeTrue();

        var suppliers = await Client.GetFromJsonAsync<IReadOnlyList<SupplierDto>>("/api/suppliers");
        suppliers.Should().NotBeNullOrEmpty();
        suppliers.Should().Contain(s => s.Id == supplierGuid);

        var deleteResponse = await Client.DeleteAsync($"/api/suppliers/{supplierGuid}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
