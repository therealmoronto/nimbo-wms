using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record AddZoneToWarehouseCommand(
    WarehouseId WarehouseId,
    AddZoneRequest Request
) : ICommand<ZoneId>;

public sealed class AddZoneToWarehouseHandler : ICommandHandler<AddZoneToWarehouseCommand, ZoneId>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddZoneToWarehouseHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ZoneId> HandleAsync(AddZoneToWarehouseCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByIdAsync(command.WarehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var zoneId = ZoneId.New();
        var request = command.Request;
        warehouse.AddZone(zoneId, request.Code, request.Name, request.Type);

        await _unitOfWork.SaveChangesAsync(ct);

        return zoneId;
    }
}
