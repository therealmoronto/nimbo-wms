using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class PatchZoneRequestHandler : IRequestHandler<PatchZoneCommand>
{
    private readonly IWarehouseRepository _repository;

    public PatchZoneRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(PatchZoneCommand command, CancellationToken ct = default)
    {
        var zoneId = ZoneId.From(command.ZoneGuid);
        var warehouse = await _repository.GetByZoneIdAsync(zoneId, ct);
        if (warehouse is null)
            throw new NotFoundException("Zone not found");
        
        var zone = warehouse.GetZone(zoneId);
        if (command.Name is not null)
            zone.Rename(command.Name);
        
        if (command.Code is not null)
            zone.ChangeCode(command.Code);
        
        if (!string.IsNullOrEmpty(command.Type) && Enum.TryParse(command.Type, out ZoneType zoneType))
            zone.ChangeType(zoneType);

        if (command.MaxWeightKg is not null || command.MaxVolumeM3 is not null)
        {
            var maxWeightKg = command.MaxWeightKg ?? zone.MaxWeightKg;
            var maxVolumeM3 = command.MaxVolumeM3 ?? zone.MaxVolumeM3;
            zone.SetCapacity(maxWeightKg, maxVolumeM3);
        }

        if (command.IsQuarantine is not null || command.IsDamagedArea is not null)
        {
            var isQuarantine = command.IsQuarantine ?? zone.IsQuarantine;
            var isDamagedArea = command.IsDamagedArea ?? zone.IsDamagedArea;
            zone.SetAreaFlags(isQuarantine, isDamagedArea);
        }
    }
}
