using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Http;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests.Stock;

[IntegrationTest]
public class BatchLifecycleApiTests : ApiTestBase
{
    public BatchLifecycleApiTests(PostgresFixture postgres)
        : base(postgres) { }

    [Fact]
    public async Task CreateBatch_GetById_ListBatches_Succeeds()
    {
        // Setup: Create an item first
        var createItemRequest = new CreateItemRequest(
            "BATCH-TEST-ITEM",
            "BTI-001",
            "00100999",
            UnitOfMeasure.Kilogram);

        var createItemResponse = await Client.PostAsJsonAsync("/api/items", createItemRequest);
        var createdItem = (await createItemResponse.Content.ReadFromJsonAsync<CreateItemResponse>())!;
        var itemId = createdItem.Id;

        // 1) Create batch
        var manufacturedAt = new DateTime(2026, 1, 15);
        DateTime.SpecifyKind(manufacturedAt, DateTimeKind.Local);

        var expiryDate = new DateTime(2027, 1, 15);
        DateTime.SpecifyKind(expiryDate, DateTimeKind.Local);

        var receivedAt = new DateTime(2026, 2, 1);
        DateTime.SpecifyKind(receivedAt, DateTimeKind.Local);

        var createBatchRequest = new CreateBatchRequest(
            ItemId: itemId,
            BatchNumber: "BATCH-2026-001",
            SupplierId: null,
            ManufacturedAt: manufacturedAt,
            ExpiryDate: expiryDate,
            ReceivedAt: receivedAt,
            Notes: "Test batch created for integration tests");

        var createBatchResponse = await Client.PostAsJsonAsync("/api/stock/batches", createBatchRequest);
        createBatchResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdBatch = (await createBatchResponse.Content.ReadFromJsonAsync<CreateBatchResponse>())!;
        var batchId = createdBatch.Id;

        // 2) Get batch by id
        var batch = await Client.GetFromJsonAsync<BatchDto>($"/api/stock/batches/{batchId}");

        batch.Should().NotBeNull();
        batch.Id.Should().Be(batchId);
        batch.ItemId.Should().Be(itemId);
        batch.BatchNumber.Should().Be("BATCH-2026-001");
        batch.ManufacturedAt.Should().Be(manufacturedAt.ToUniversalTime());
        batch.ExpiryDate.Should().Be(expiryDate.ToUniversalTime());
        batch.ReceivedAt.Should().Be(receivedAt.ToUniversalTime());
        batch.Notes.Should().Be("Test batch created for integration tests");

        var batches = await Client.GetFromJsonAsync<IReadOnlyList<BatchDto>>($"/api/stock/batches?ItemGuid={itemId}");

        batches.Should().NotBeNullOrEmpty();

        var listedBatch = batches.Single(b => b.Id == batchId);
        listedBatch.ItemId.Should().Be(itemId);
    }
}
