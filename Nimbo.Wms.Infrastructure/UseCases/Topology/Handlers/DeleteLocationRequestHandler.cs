using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed class DeleteLocationRequestHandler : IRequestHandler<DeleteLocationRequest>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteLocationRequestHandler(IWarehouseRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteLocationRequest request, CancellationToken ct = default)
    {
        var locationId = LocationId.From(request.LocationGuid);
        var warehouse = await _repository.GetByLocationIdAsync(locationId, ct);
        if (warehouse == null)
            throw new NotFoundException($"Warehouse with location id {request.LocationGuid} does not exist");

        warehouse.RemoveLocation(locationId);

        await _uow.CommitAsync(ct);
    }
}
