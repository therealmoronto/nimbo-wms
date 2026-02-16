using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record DeleteZoneCommand(ZoneId ZoneId) : ICommand;

public sealed class DeleteZoneHandler : ICommandHandler<DeleteZoneCommand>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteZoneHandler(IWarehouseRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task HandleAsync(DeleteZoneCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByZoneIdAsync(command.ZoneId, ct);
        if (warehouse == null)
            throw new NotFoundException($"Warehouse with zone id {command.ZoneId} does not exist");

        warehouse.RemoveZone(command.ZoneId);

        await _uow.SaveChangesAsync(ct);
    }
}
