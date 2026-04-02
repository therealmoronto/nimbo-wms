using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class PatchLocationRequestHandler : IRequestHandler<PatchLocationRequest>
{
    private readonly IWarehouseRepository _repository;

    public PatchLocationRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchLocationRequest request, CancellationToken ct = default)
    {
        var locationId = LocationId.From(request.LocationGuid);
        var warehouse = await _repository.GetByLocationIdAsync(locationId, ct);
        if (warehouse is null)
            throw new NotFoundException("Location not found");

        var location = warehouse.GetLocation(locationId);
        // Code / Type
        if (request.Code is not null)
            location.ChangeCode(request.Code);

        if (request.Type.HasValue)
            location.ChangeType(request.Type.Value);
        
        // Capacity: apply only fields that are present; keep others unchanged
        if (request.MaxWeightKg is not null || request.MaxVolumeM3 is not null)
        {
            var maxWeight = request.MaxWeightKg ?? location.MaxWeightKg;
            var maxVolume = request.MaxVolumeM3 ?? location.MaxVolumeM3;
            location.SetCapacity(maxWeight, maxVolume);
        }

        // Address parts: apply only fields that are present; keep others unchanged
        if (request.Aisle is not null || request.Rack is not null || request.Level is not null || request.Position is not null)
        {
            var aisle = request.Aisle ?? location.Aisle;
            var rack = request.Rack ?? location.Rack;
            var level = request.Level ?? location.Level;
            var position = request.Position ?? location.Position;

            location.SetAddressParts(aisle, rack, level, position);
        }

        // Usage flags: apply only fields that are present; keep others unchanged
        if (request.IsSingleItemOnly.HasValue ||
            request.IsPickingLocation.HasValue ||
            request.IsReceivingLocation.HasValue ||
            request.IsShippingLocation.HasValue)
        {
            var single = request.IsSingleItemOnly ?? location.IsSingleItemOnly;
            var picking = request.IsPickingLocation ?? location.IsPickingLocation;
            var receiving = request.IsReceivingLocation ?? location.IsReceivingLocation;
            var shipping = request.IsShippingLocation ?? location.IsShippingLocation;

            location.SetUsageFlags(single, picking, receiving, shipping);
        }

        // Active / Blocked
        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value) location.Activate();
            else location.Deactivate();
        }

        if (request.IsBlocked.HasValue)
        {
            if (request.IsBlocked.Value) location.Block();
            else location.Unblock();
        }
    }
}
