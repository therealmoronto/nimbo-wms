using System.Diagnostics.CodeAnalysis;
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
[SuppressMessage("Usage", "xUnit1041:Fixture arguments to test classes must have fixture sources")]
public class WarehouseTopologyLifecycleApiTests : ApiTestBase
{
    public WarehouseTopologyLifecycleApiTests(PostgresFixture postgres)
        : base(postgres) { }
    
    [Fact]
    public async Task CreateWarehouse_AddZone_AddLocation_Then_GetTopology()
    {
        // 1) Create warehouse
        var createWarehouse = new CreateWarehouseRequest(
            Code: $"WH-{Guid.NewGuid():N}".Substring(0, 10),
            Name: "Main Warehouse");

        var createWarehouseResponse = await Client.PostAsJsonAsync("/api/topology/warehouses", createWarehouse);

        createWarehouseResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdWarehouse = await createWarehouseResponse.Content.ReadFromJsonAsync<CreateWarehouseResponse>();

        createdWarehouse.Should().NotBeNull();
        var warehouseGuid = createdWarehouse.Id;

        // 2) Add zone
        var addZone = new AddZoneRequest(
            Code: "Z-A",
            Name: "Zone A",
            Type: ZoneType.Storage);

        var addZoneResponse = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/zones", addZone);

        addZoneResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdZone = await addZoneResponse.Content.ReadFromJsonAsync<AddZoneResponse>();
        createdZone.Should().NotBeNull();
        var zoneGuid = createdZone.ZoneId;

        // 3) Add location
        var addLocation = new AddLocationRequest(
            zoneGuid,
            Code: "A-01-01-01",
            Type: LocationType.Shelf);

        var addLocationResponse = await Client.PostAsJsonAsync($"/api/topology/warehouses/{warehouseGuid}/locations", addLocation);

        addLocationResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdLocation = await addLocationResponse.Content.ReadFromJsonAsync<AddLocationResponse>();
        createdLocation.Should().NotBeNull();

        // 4) Get topology
        var topology = await Client.GetFromJsonAsync<WarehouseTopologyDto>($"/api/topology/warehouses/{warehouseGuid}");

        topology.Should().NotBeNull();
        topology.Id.Should().Be(warehouseGuid);

        topology.Zones.Should().ContainSingle(z => z.Id == zoneGuid);
        topology.Locations.Should().ContainSingle(l => l.ZoneId == zoneGuid && l.Code == "A-01-01-01");

        // 5) Get warehouses list
        var warehouses = await Client.GetFromJsonAsync<IReadOnlyList<WarehouseListItemDto>>("/api/topology/warehouses");
        warehouses.Should().NotBeNull();
        warehouses.Should().ContainSingle(w => w.Id == warehouseGuid);
    }
}
