using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record DeleteWarehouseCommand(WarehouseId WarehouseId) : ICommand;

public sealed class DeleteWarehouseHandler : ICommandHandler<DeleteWarehouseCommand>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteWarehouseHandler(IWarehouseRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task HandleAsync(DeleteWarehouseCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByIdAsync(command.WarehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException("Warehouse not found");
        
        warehouse.EnsureCanBeDeleted();
        await _repository.DeleteAsync(warehouse, ct);

        await _uow.SaveChangesAsync(ct);
    }
}
