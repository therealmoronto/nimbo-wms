using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record AddLocationToWarehouseCommand(
    WarehouseId WarehouseId,
    ZoneId ZoneId,
    string Code,
    LocationType Type
) : ICommand<LocationId>;

public sealed class AddLocationToWarehouseHandler : ICommandHandler<AddLocationToWarehouseCommand, LocationId>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddLocationToWarehouseHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LocationId> HandleAsync(AddLocationToWarehouseCommand command, CancellationToken ct = default)
    {
        
        var warehouse = await _repository.GetByIdAsync(command.WarehouseId);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var locationId = LocationId.New();
        warehouse.AddLocation(locationId, command.ZoneId, command.Code, command.Type);

        await _unitOfWork.SaveChangesAsync(ct);

        return locationId;
    }
}
