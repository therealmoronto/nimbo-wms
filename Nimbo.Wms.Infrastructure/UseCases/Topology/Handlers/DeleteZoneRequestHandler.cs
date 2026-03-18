using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed class DeleteZoneRequestHandler : IRequestHandler<DeleteZoneRequest>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteZoneRequestHandler(IWarehouseRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteZoneRequest request, CancellationToken ct = default)
    {
        var zoneId = ZoneId.From(request.ZoneGuid);
        var warehouse = await _repository.GetByZoneIdAsync(zoneId, ct);
        if (warehouse == null)
            throw new NotFoundException($"Warehouse with zone id {request.ZoneGuid} does not exist");

        warehouse.RemoveZone(zoneId);

        await _uow.CommitAsync(ct);
    }
}
