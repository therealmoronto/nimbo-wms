using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class PatchWarehouseRequestHandler : IRequestHandler<PatchWarehouseCommand>
{
    private readonly IWarehouseRepository _repository;

    public PatchWarehouseRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }


    public async Task Handle(PatchWarehouseCommand command, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(command.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        warehouse.Rename(command.Name ?? warehouse.Name);
        warehouse.ChangeCode(command.Code ?? warehouse.Code);
        warehouse.ChangeAddress(command.Address ?? warehouse.Address);
        warehouse.ChangeDescription(command.Description ?? warehouse.Description);
    }
}
