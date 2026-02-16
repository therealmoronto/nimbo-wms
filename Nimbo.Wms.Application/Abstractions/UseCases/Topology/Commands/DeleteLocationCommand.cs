using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record DeleteLocationCommand(LocationId LocationId) : ICommand;

public sealed class DeleteLocationHandler : ICommandHandler<DeleteLocationCommand>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteLocationHandler(IWarehouseRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task HandleAsync(DeleteLocationCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByLocationIdAsync(command.LocationId, ct);
        if (warehouse == null)
            throw new NotFoundException($"Warehouse with zone id {command.LocationId} does not exist");

        warehouse.RemoveLocation(command.LocationId);

        await _uow.SaveChangesAsync(ct);
    }
}
