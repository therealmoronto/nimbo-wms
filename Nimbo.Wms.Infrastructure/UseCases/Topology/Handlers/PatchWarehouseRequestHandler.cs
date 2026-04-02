using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class PatchWarehouseRequestHandler : IRequestHandler<PatchWarehouseRequest>
{
    private readonly IWarehouseRepository _repository;

    public PatchWarehouseRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }


    public async Task Handle(PatchWarehouseRequest request, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(request.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        warehouse.Rename(request.Name ?? warehouse.Name);
        warehouse.ChangeCode(request.Code ?? warehouse.Code);
        warehouse.ChangeAddress(request.Address ?? warehouse.Address);
        warehouse.ChangeDescription(request.Description ?? warehouse.Description);
    }
}
