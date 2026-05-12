using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class AddZoneCommandHandler : IRequestHandler<AddZoneCommand, Guid>
{
    private readonly IWarehouseRepository _repository;

    public AddZoneCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(AddZoneCommand command, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(command.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var zoneId = ZoneId.New();
        warehouse.AddZone(zoneId, command.Code, command.Name, Enum.Parse<ZoneType>(command.Type));

        return zoneId;
    }
}
