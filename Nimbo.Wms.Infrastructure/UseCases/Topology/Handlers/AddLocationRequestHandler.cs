using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class AddLocationRequestHandler : IRequestHandler<AddLocationRequest, Guid>
{
    private readonly IWarehouseRepository _repository;

    public AddLocationRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddLocationRequest request, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(request.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var locationId = LocationId.New();
        var zoneId = ZoneId.From(request.ZoneGuid);
        warehouse.AddLocation(locationId, zoneId, request.Code, Enum.Parse<LocationType>(request.Type));

        return locationId;
    }
}
