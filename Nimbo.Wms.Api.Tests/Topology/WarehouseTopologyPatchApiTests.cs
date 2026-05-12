using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.References;
using Nimbo.Wms.Models.Topology;
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
        var createWarehouseRequest = new CreateWarehouseCommand($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main");
        var createRes = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);

        createRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = (await createRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseGuid = created.Id;

        // update
        var patchWarehouseRequest = new PatchWarehouseCommand(warehouseGuid, "WH-UPDATED", "Main Updated", "Address lane 1, 0015", "Test warehouse to test patch API");
        var updateRes = await Client.PatchAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}", patchWarehouseRequest);

        updateRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // read topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseGuid}");

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
        var createWarehouseRequest = new CreateWarehouseCommand($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main");
        var whRes = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseGuid = wh.Id;

        // add zone
        var addZoneRequest = new AddZoneCommand(warehouseGuid, "Z-A", "Zone A", nameof(ZoneType.Storage));
        var zRes = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/zones", addZoneRequest);

        zRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneGuid = zone.ZoneId;

        // patch zone
        var patch = new PatchZoneCommand(
            zoneGuid,
            Name: "Zone A (Updated)",
            IsQuarantine: true,
            MaxWeightKg: 1234m
        );

        var patchRes = await Client.PatchAsJsonAsync($"/api/topology/zones/{zoneGuid}", patch);
        patchRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // read topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseGuid}");
        topology.Should().NotBeNull();

        var updated = topology.Zones.Single(x => x.Id == zoneGuid);
        updated.Name.Should().Be("Zone A (Updated)");
        updated.IsQuarantine.Should().BeTrue();
        updated.MaxWeightKg.Should().Be(1234m);
    }

    [Fact]
    public async Task PatchLocation_UpdatesFields_VisibleInTopology()
    {
        // create warehouse
        var createWarehouseRequest = new CreateWarehouseCommand($"WH-{Guid.NewGuid():N}".Substring(0, 10), "Main");
        var whRes = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouseRequest);

        whRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var wh = (await whRes.Content.ReadFromJsonAsync<CreateWarehouseResponse>())!;
        var warehouseGuid = wh.Id;

        // add zone
        var addZoneRequest = new AddZoneCommand(warehouseGuid, "Z-A", "Zone A", nameof(ZoneType.Storage));
        var zRes = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/zones", addZoneRequest);

        zRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var zone = (await zRes.Content.ReadFromJsonAsync<AddZoneResponse>())!;
        var zoneGuid = zone.ZoneId;

        // add location
        var addLocationRequest = new AddLocationCommand(warehouseGuid, zoneGuid, "A-01-01-01", nameof(LocationType.Shelf));
        var lRes = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/locations", addLocationRequest);

        lRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var loc = (await lRes.Content.ReadFromJsonAsync<AddLocationResponse>())!;
        var locationGuid = loc.LocationId;

        // patch location
        var patch = new PatchLocationCommand(
            locationGuid,
            IsPickingLocation: true,
            IsBlocked: true,
            Aisle: "A",
            Rack: "01",
            Level: "01",
            Position: "01"
        );

        var patchRes = await Client.PatchAsJsonAsync($"/api/topology/locations/{locationGuid}", patch);
        patchRes.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // read topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseGuid}");
        topology.Should().NotBeNull();

        var updated = topology.Locations.Single(x => x.Id == locationGuid);
        updated.IsPickingLocation.Should().BeTrue();
        updated.IsBlocked.Should().BeTrue();
        updated.Aisle.Should().Be("A");
        updated.Rack.Should().Be("01");
        updated.Level.Should().Be("01");
        updated.Position.Should().Be("01");
    }
}
