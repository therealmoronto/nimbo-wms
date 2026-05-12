using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand>
{
    private readonly IWarehouseRepository _repository;

    public DeleteLocationCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteLocationCommand command, CancellationToken ct = default)
    {
        var locationId = LocationId.From(command.LocationGuid);
        var warehouse = await _repository.GetByLocationIdAsync(locationId, ct);
        if (warehouse == null)
            throw new NotFoundException($"Warehouse with location id {command.LocationGuid} does not exist");

        warehouse.RemoveLocation(locationId);
    }
}
