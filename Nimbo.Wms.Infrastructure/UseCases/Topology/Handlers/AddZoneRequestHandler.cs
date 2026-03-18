using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed class AddZoneRequestHandler : IRequestHandler<AddZoneRequest, ZoneId>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddZoneRequestHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ZoneId> Handle(AddZoneRequest request, CancellationToken ct = default)
    {
        var warehouseId = WarehouseId.From(request.WarehouseGuid);
        var warehouse = await _repository.GetByIdAsync(warehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var zoneId = ZoneId.New();
        warehouse.AddZone(zoneId, request.Code, request.Name, request.Type);

        await _unitOfWork.CommitAsync(ct);

        return zoneId;
    }
}
