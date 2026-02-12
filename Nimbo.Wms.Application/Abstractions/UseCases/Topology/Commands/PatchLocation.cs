using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record PatchLocationCommand(
    LocationId LocationId,
    PatchLocationRequest Patch
) : ICommand;

public sealed class PatchLocationHandler : ICommandHandler<PatchLocationCommand>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchLocationHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(PatchLocationCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByLocationIdAsync(command.LocationId, ct);
        if (warehouse is null)
            throw new NotFoundException("Location not found");

        var location = warehouse.GetLocation(command.LocationId);
        var patch = command.Patch;

        // Code / Type
        if (patch.Code is not null)
            location.ChangeCode(patch.Code);

        if (patch.Type.HasValue)
            location.ChangeType(patch.Type.Value);
        
        // Capacity: apply only fields that are present; keep others unchanged
        if (patch.MaxWeightKg is not null || patch.MaxVolumeM3 is not null)
        {
            var maxWeight = patch.MaxWeightKg ?? location.MaxWeightKg;
            var maxVolume = patch.MaxVolumeM3 ?? location.MaxVolumeM3;
            location.SetCapacity(maxWeight, maxVolume);
        }

        // Address parts: apply only fields that are present; keep others unchanged
        if (patch.Aisle is not null || patch.Rack is not null || patch.Level is not null || patch.Position is not null)
        {
            var aisle = patch.Aisle ?? location.Aisle;
            var rack = patch.Rack ?? location.Rack;
            var level = patch.Level ?? location.Level;
            var position = patch.Position ?? location.Position;

            location.SetAddressParts(aisle, rack, level, position);
        }

        // Usage flags: apply only fields that are present; keep others unchanged
        if (patch.IsSingleItemOnly.HasValue ||
            patch.IsPickingLocation.HasValue ||
            patch.IsReceivingLocation.HasValue ||
            patch.IsShippingLocation.HasValue)
        {
            var single = patch.IsSingleItemOnly ?? location.IsSingleItemOnly;
            var picking = patch.IsPickingLocation ?? location.IsPickingLocation;
            var receiving = patch.IsReceivingLocation ?? location.IsReceivingLocation;
            var shipping = patch.IsShippingLocation ?? location.IsShippingLocation;

            location.SetUsageFlags(single, picking, receiving, shipping);
        }

        // Active / Blocked
        if (patch.IsActive.HasValue)
        {
            if (patch.IsActive.Value) location.Activate();
            else location.Deactivate();
        }

        if (patch.IsBlocked.HasValue)
        {
            if (patch.IsBlocked.Value) location.Block();
            else location.Unblock();
        }

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
