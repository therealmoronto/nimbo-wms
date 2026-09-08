using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, Result>
{
    private readonly IWarehouseRepository _repository;

    public DeleteWarehouseCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteWarehouseCommand command, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(command.WarehouseId);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse == null)
            return Error.NotFound("warehouse.notfound", "Warehouse not found");

        warehouse.EnsureCanBeDeleted();
        await _repository.DeleteAsync(warehouse, ct);

        return Result.Success();
    }
}
