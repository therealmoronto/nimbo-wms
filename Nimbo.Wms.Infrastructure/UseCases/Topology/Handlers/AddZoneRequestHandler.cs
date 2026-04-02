using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class AddZoneRequestHandler : IRequestHandler<AddZoneRequest, ZoneId>
{
    private readonly IWarehouseRepository _repository;

    public AddZoneRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<ZoneId> Handle(AddZoneRequest request, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(request.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var zoneId = ZoneId.New();
        warehouse.AddZone(zoneId, request.Code, request.Name, request.Type);

        return zoneId;
    }
}
