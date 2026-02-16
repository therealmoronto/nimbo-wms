using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record PatchZoneCommand(
    ZoneId ZoneId,
    PatchZoneRequest Patch
) : ICommand;

public sealed class PatchZoneHandler : ICommandHandler<PatchZoneCommand>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchZoneHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task HandleAsync(PatchZoneCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByZoneIdAsync(command.ZoneId, ct);
        if (warehouse is null)
            throw new NotFoundException("Zone not found");
        
        var zone = warehouse.GetZone(command.ZoneId);
        var patch = command.Patch;
        
        if (patch.Name is not null)
            zone.Rename(patch.Name);
        
        if (patch.Code is not null)
            zone.ChangeCode(patch.Code);
        
        if (patch.Type is not null)
            zone.ChangeType(patch.Type.Value);

        if (patch.MaxWeightKg is not null || patch.MaxVolumeM3 is not null)
        {
            var maxWeightKg = patch.MaxWeightKg ?? zone.MaxWeightKg;
            var maxVolumeM3 = patch.MaxVolumeM3 ?? zone.MaxVolumeM3;
            zone.SetCapacity(maxWeightKg, maxVolumeM3);
        }

        if (patch.IsQuarantine is not null || patch.IsDamagedArea is not null)
        {
            var isQuarantine = patch.IsQuarantine ?? zone.IsQuarantine;
            var isDamagedArea = patch.IsDamagedArea ?? zone.IsDamagedArea;
            zone.SetAreaFlags(isQuarantine, isDamagedArea);
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
