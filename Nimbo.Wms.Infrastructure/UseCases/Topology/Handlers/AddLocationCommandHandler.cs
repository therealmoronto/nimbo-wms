using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class AddLocationCommandHandler : IRequestHandler<AddLocationCommand, Result<Guid>>
{
    private readonly IWarehouseRepository _repository;

    public AddLocationCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(AddLocationCommand command, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(command.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            return Error.NotFound("warehouse.notfound", "Warehouse not found");

        var locationId = LocationId.New();
        var zoneId = ZoneId.From(command.ZoneGuid);
        warehouse.AddLocation(locationId, zoneId, command.Code, Enum.Parse<LocationType>(command.Type));

        return locationId.Value;
    }
}
