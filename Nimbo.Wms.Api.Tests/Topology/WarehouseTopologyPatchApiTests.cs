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
public class WarehouseTopologyPatchApiTests : ApiTestBase
{
    public WarehouseTopologyPatchApiTests(PostgresFixture postgres)
        : base(postgres) { }

    [Fact]
    public async Task UpdateWarehouse_ChangesAreVisibleInTopology()
    {
        // create warehouse
        var createWarehouseRequest = new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main");
        var createRes = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);

        createRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = (await createRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = created.Id;

        // update
        var patchWarehouseRequest = new PatchWarehouseRequest("WH-UPDATED", "Main Updated", "Address lane 1, 0015", "Test warehouse to test patch API");
        var updateRes = await Client.PatchAsJsonAsync($"/api/topology/warehouses/{warehouseId}", patchWarehouseRequest);

        updateRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // read topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseId}");

        topology.Should().NotBeNull();
        topology.Code.Should().Be("WH-UPDATED");
        topology.Name.Should().Be("Main Updated");
        topology.Address.Should().Be("Address lane 1, 0015");
        topology.Description.Should().Be("Test warehouse to test patch API");
    }

    [Fact]
    public async Task PatchZone_UpdatesFields_VisibleInTopology()
    {
        // create warehouse
        var createWarehouseRequest = new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main");
        var whRes = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = wh.Id;

        // add zone
        var addZoneRequest = new AddZoneRequest("Z-A", "Zone A", ZoneType.Storage);
        var zRes = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseId}/zones", addZoneRequest);

        zRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneId = zone.ZoneId;

        // patch zone
        var patch = new PatchZoneRequest(
            Name: "Zone A (Updated)",
            IsQuarantine: true,
            MaxWeightKg: 1234m
        );

        var patchRes = await Client.PatchAsJsonAsync($"/api/topology/zones/{zoneId}", patch);
        patchRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // read topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseId}");
        topology.Should().NotBeNull();

        var updated = topology.Zones.Single(x => x.Id == zoneId);
        updated.Name.Should().Be("Zone A (Updated)");
        updated.IsQuarantine.Should().BeTrue();
        updated.MaxWeightKg.Should().Be(1234m);
    }

    [Fact]
    public async Task PatchLocation_UpdatesFields_VisibleInTopology()
    {
        // create warehouse
        var createWarehouseRequest = new CreateWarehouseRequest($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main");
        var whRes = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseId = wh.Id;

        // add zone
        var addZoneRequest = new AddZoneRequest("Z-A", "Zone A", ZoneType.Storage);
        var zRes = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseId}/zones", addZoneRequest);

        zRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneId = zone.ZoneId;

        // add location
        var addLocationRequest = new AddLocationRequest(zoneId, "A-01-01-01", LocationType.Shelf);
        var lRes = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseId}/locations", addLocationRequest);

        lRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var loc = (await lRes.Content.ReadFromJsonAsync<AddLocationResponse>())!;
        var locationId = loc.LocationId;

        // patch location
        var patch = new PatchLocationRequest(
            IsPickingLocation: true,
            IsBlocked: true,
            Aisle: "A",
            Rack: "01",
            Level: "01",
            Position: "01"
        );

        var patchRes = await Client.PatchAsJsonAsync($"/api/topology/locations/{locationId}", patch);
        patchRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // read topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseId}");
        topology.Should().NotBeNull();

        var updated = topology.Locations.Single(x => x.Id == locationId);
        updated.IsPickingLocation.Should().BeTrue();
        updated.IsBlocked.Should().BeTrue();
        updated.Aisle.Should().Be("A");
        updated.Rack.Should().Be("01");
        updated.Level.Should().Be("01");
        updated.Position.Should().Be("01");
    }
}
