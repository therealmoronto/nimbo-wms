using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class DeleteZoneCommandHandler : IRequestHandler<DeleteZoneCommand, Result>
{
    private readonly IWarehouseRepository _repository;

    public DeleteZoneCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteZoneCommand command, CancellationToken ct = default)
    {
        var zoneId = ZoneId.From(command.ZoneGuid);
        var warehouse = await _repository.GetByZoneIdAsync(zoneId, ct);
        if (warehouse == null)
            return Error.NotFound("warehouse.notfound", $"Warehouse with zone id {command.ZoneGuid} does not exist");

        warehouse.RemoveZone(zoneId);
        return Result.Success();
    }
}
