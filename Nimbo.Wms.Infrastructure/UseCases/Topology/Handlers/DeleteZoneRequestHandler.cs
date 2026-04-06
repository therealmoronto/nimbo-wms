using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class DeleteZoneRequestHandler : IRequestHandler<DeleteZoneRequest>
{
    private readonly IWarehouseRepository _repository;

    public DeleteZoneRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteZoneRequest request, CancellationToken ct = default)
    {
        var zoneId = ZoneId.From(request.ZoneGuid);
        var warehouse = await _repository.GetByZoneIdAsync(zoneId, ct);
        if (warehouse == null)
            throw new NotFoundException($"Warehouse with zone id {request.ZoneGuid} does not exist");

        warehouse.RemoveZone(zoneId);
    }
}
