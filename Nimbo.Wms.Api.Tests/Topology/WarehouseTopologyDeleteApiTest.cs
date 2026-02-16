using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Tests.Common.Attributes;
using Nimbo.Wms.Tests.Common.Database;

namespace Nimbo.Wms.Api.Tests.Topology;

[IntegrationTest]
public class WarehouseTopologyDeleteApiTest : ApiTestBase
{
    public WarehouseTopologyDeleteApiTest(PostgresFixture postgres)
        : base(postgres) { }

#region Warehouse deletion tests
    [Fact]
    public async Task DeleteWarehouse_WhenEmpty_Returns204_AndGetReturns404()
    {
        var createRes = await Client.PostAsJsonAsync(
            "/api/topology/warehouses",
            new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main"));

        createRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = (await createRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = created.Id;

        var deleteRes = await Client.DeleteAsync($"/api/topology/warehouses/{warehouseId}");
        deleteRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getRes = await Client.GetAsync($"/api/topology/warehouses/{warehouseId}");
        getRes.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteWarehouse_WhenNotEmpty_Returns400()
    {
        // Create warehouse
        var createRes = await Client.PostAsJsonAsync(
            "/api/topology/warehouses",
            new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main"));

        createRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = (await createRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = created.Id;

        // Add zone -> warehouse becomes non-empty
        var zoneRes = await Client.PostAsJsonAsync(
            $"/api/topology/warehouses/{warehouseId}/zones",
            new AddZoneRequest("Z-A", "Zone A", ZoneType.Storage));

        zoneRes.StatusCode.Should().Be(HttpStatusCode.Created);

        // Delete should fail
        var deleteRes = await Client.DeleteAsync($"/api/topology/warehouses/{warehouseId}");
        deleteRes.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteWarehouse_UnknownId_Returns404()
    {
        var id = Guid.NewGuid();
        var res = await Client.DeleteAsync($"/api/topology/warehouses/{id}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
#endregion Warehouse deletion tests

#region Zone deletion tests
    [Fact]
    public async Task DeleteZone_WhenEmpty_Returns204_AndZoneDisappearsFromTopology()
    {
        // Create warehouse
        var whRes = await Client.PostAsJsonAsync(
            "/api/topology/warehouses",
            new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main"));

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = wh.Id;

        // Add zone
        var zoneRes = await Client.PostAsJsonAsync(
            $"/api/topology/warehouses/{warehouseId}/zones",
            new AddZoneRequest("Z-A", "Zone A", ZoneType.Storage));

        zoneRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zoneRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneId = zone.ZoneId;

        // Delete zone
        var deleteRes = await Client.DeleteAsync($"/api/topology/zones/{zoneId}");
        deleteRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>(
            $"/api/topology/warehouses/{warehouseId}");

        topology.Should().NotBeNull();
        topology.Zones.Should().NotContain(z => z.Id == zoneId);
    }

    [Fact]
    public async Task DeleteZone_WhenHasLocations_Returns400()
    {
        // Create warehouse
        var whRes = await Client.PostAsJsonAsync(
            "/api/topology/warehouses",
            new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main"));

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = wh.Id;

        // Add zone
        var zoneRes = await Client.PostAsJsonAsync(
            $"/api/topology/warehouses/{warehouseId}/zones",
            new AddZoneRequest("Z-A", "Zone A", ZoneType.Storage));

        zoneRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zoneRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneId = zone.ZoneId;

        // Add location in that zone
        var locRes = await Client.PostAsJsonAsync(
            $"/api/topology/warehouses/{warehouseId}/locations",
            new AddLocationRequest(zoneId, "A-01-01-01", LocationType.Shelf));

        locRes.StatusCode.Should().Be(HttpStatusCode.Created);

        // Delete zone -> should fail
        var deleteRes = await Client.DeleteAsync($"/api/topology/zones/{zoneId}");
        deleteRes.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteZone_UnknownId_Returns404()
    {
        var id = Guid.NewGuid();
        var res = await Client.DeleteAsync($"/api/topology/zones/{id}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
#endregion Zone deletion tests
    
#region Location deletion tests
    [Fact]
    public async Task DeleteLocation_Returns204_AndLocationDisappearsFromTopology()
    {
        // Create warehouse
        var whRes = await Client.PostAsJsonAsync(
            "/api/topology/warehouses",
            new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main"));

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = wh.Id;

        // Add zone
        var zoneRes = await Client.PostAsJsonAsync(
            $"/api/topology/warehouses/{warehouseId}/zones",
            new AddZoneRequest("Z-A", "Zone A", ZoneType.Storage));

        zoneRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zoneRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneId = zone.ZoneId;

        // Add location
        var locRes = await Client.PostAsJsonAsync(
            $"/api/topology/warehouses/{warehouseId}/locations",
            new AddLocationRequest(zoneId, "A-01-01-01", LocationType.Shelf));

        locRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var loc = (await locRes.Content.ReadFromJsonAsync<AddLocationResponse>())!;
        var locationId = loc.LocationId;

        // Delete location
        var deleteRes = await Client.DeleteAsync($"/api/topology/locations/{locationId}");
        deleteRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>(
            $"/api/topology/warehouses/{warehouseId}");

        topology.Should().NotBeNull();
        topology.Locations.Should().NotContain(l => l.Id == locationId);
    }

    [Fact]
    public async Task DeleteLocation_UnknownId_Returns404()
    {
        var id = Guid.NewGuid();
        var res = await Client.DeleteAsync($"/api/topology/locations/{id}");
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
#endregion Location deletion tests
}
