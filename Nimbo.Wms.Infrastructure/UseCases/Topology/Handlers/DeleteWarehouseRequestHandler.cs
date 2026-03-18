using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class DeleteWarehouseRequestHandler : IRequestHandler<DeleteWarehouseRequest>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteWarehouseRequestHandler(IWarehouseRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteWarehouseRequest request, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByIdAsync(request.WarehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException("Warehouse not found");
        
        warehouse.EnsureCanBeDeleted();
        await _repository.DeleteAsync(warehouse, ct);

        await _uow.CommitAsync(ct);
    }
}
