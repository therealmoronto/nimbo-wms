using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class PatchZoneRequestHandler : IRequestHandler<PatchZoneRequest>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchZoneRequestHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(PatchZoneRequest request, CancellationToken ct = default)
    {
        var zoneId = ZoneId.From(request.ZoneGuid);
        var warehouse = await _repository.GetByZoneIdAsync(zoneId, ct);
        if (warehouse is null)
            throw new NotFoundException("Zone not found");
        
        var zone = warehouse.GetZone(zoneId);
        if (request.Name is not null)
            zone.Rename(request.Name);
        
        if (request.Code is not null)
            zone.ChangeCode(request.Code);
        
        if (request.Type is not null)
            zone.ChangeType(request.Type.Value);

        if (request.MaxWeightKg is not null || request.MaxVolumeM3 is not null)
        {
            var maxWeightKg = request.MaxWeightKg ?? zone.MaxWeightKg;
            var maxVolumeM3 = request.MaxVolumeM3 ?? zone.MaxVolumeM3;
            zone.SetCapacity(maxWeightKg, maxVolumeM3);
        }

        if (request.IsQuarantine is not null || request.IsDamagedArea is not null)
        {
            var isQuarantine = request.IsQuarantine ?? zone.IsQuarantine;
            var isDamagedArea = request.IsDamagedArea ?? zone.IsDamagedArea;
            zone.SetAreaFlags(isQuarantine, isDamagedArea);
        }

        await _unitOfWork.CommitAsync(ct);
    }
}
