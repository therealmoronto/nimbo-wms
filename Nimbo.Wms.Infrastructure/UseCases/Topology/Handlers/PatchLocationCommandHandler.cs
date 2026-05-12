using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class PatchLocationCommandHandler : IRequestHandler<PatchLocationCommand>
{
    private readonly IWarehouseRepository _repository;

    public PatchLocationCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchLocationCommand command, CancellationToken ct = default)
    {
        var locationId = LocationId.From(command.LocationGuid);
        var warehouse = await _repository.GetByLocationIdAsync(locationId, ct);
        if (warehouse is null)
            throw new NotFoundException("Location not found");

        var location = warehouse.GetLocation(locationId);
        // Code / Type
        if (command.Code is not null)
            location.ChangeCode(command.Code);

        if (!string.IsNullOrEmpty(command.Type) && Enum.TryParse(command.Type, out LocationType locationType))
            location.ChangeType(locationType);
        
        // Capacity: apply only fields that are present; keep others unchanged
        if (command.MaxWeightKg is not null || command.MaxVolumeM3 is not null)
        {
            var maxWeight = command.MaxWeightKg ?? location.MaxWeightKg;
            var maxVolume = command.MaxVolumeM3 ?? location.MaxVolumeM3;
            location.SetCapacity(maxWeight, maxVolume);
        }

        // Address parts: apply only fields that are present; keep others unchanged
        if (command.Aisle is not null || command.Rack is not null || command.Level is not null || command.Position is not null)
        {
            var aisle = command.Aisle ?? location.Aisle;
            var rack = command.Rack ?? location.Rack;
            var level = command.Level ?? location.Level;
            var position = command.Position ?? location.Position;

            location.SetAddressParts(aisle, rack, level, position);
        }

        // Usage flags: apply only fields that are present; keep others unchanged
        if (command.IsSingleItemOnly.HasValue ||
            command.IsPickingLocation.HasValue ||
            command.IsReceivingLocation.HasValue ||
            command.IsShippingLocation.HasValue)
        {
            var single = command.IsSingleItemOnly ?? location.IsSingleItemOnly;
            var picking = command.IsPickingLocation ?? location.IsPickingLocation;
            var receiving = command.IsReceivingLocation ?? location.IsReceivingLocation;
            var shipping = command.IsShippingLocation ?? location.IsShippingLocation;

            location.SetUsageFlags(single, picking, receiving, shipping);
        }

        // Active / Blocked
        if (command.IsActive.HasValue)
        {
            if (command.IsActive.Value) location.Activate();
            else location.Deactivate();
        }

        if (command.IsBlocked.HasValue)
        {
            if (command.IsBlocked.Value) location.Block();
            else location.Unblock();
        }
    }
}
