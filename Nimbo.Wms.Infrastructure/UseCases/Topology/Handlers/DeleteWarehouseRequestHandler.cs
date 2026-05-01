using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class DeleteWarehouseRequestHandler : IRequestHandler<DeleteWarehouseRequest>
{
    private readonly IWarehouseRepository _repository;

    public DeleteWarehouseRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteWarehouseRequest request, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(request.WarehouseId);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException("Warehouse not found");
        
        warehouse.EnsureCanBeDeleted();
        await _repository.DeleteAsync(warehouse, ct);
    }
}
