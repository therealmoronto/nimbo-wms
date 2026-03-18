using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class PatchItemRequestHandler : IRequestHandler<PatchItemRequest>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _uow;

    public PatchItemRequestHandler(IItemRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }
    
    public async Task Handle(PatchItemRequest request, CancellationToken ct = default)
    {
        var itemId = ItemId.From(request.ItemGuid);
        var item = await _repository.GetByIdAsync(itemId, ct);
        if (item is null)
            throw new NotFoundException($"Item with id {itemId} not found");
        
        if (!string.IsNullOrWhiteSpace(request.Name))
            item.Rename(request.Name);
        
        if (!string.IsNullOrWhiteSpace(request.InternalSku))
            item.ChangeInternalSku(request.InternalSku);
        
        if (!string.IsNullOrWhiteSpace(request.Barcode))
            item.ChangeBarcode(request.Barcode);
        
        if (request.BaseUom is not null)
            item.ChangeBaseUom(request.BaseUom.Value);

        if (!string.IsNullOrWhiteSpace(request.Manufacturer))
            item.ChangeManufacturer(request.Manufacturer);

        if (request.WeightKg is not null || request.VolumeM3 is not null)
        {
            var weight = request.WeightKg ?? item.WeightKg;
            var volume = request.VolumeM3 ?? item.VolumeM3;
            item.SetPhysical(weight, volume);
        }

        await _uow.CommitAsync(ct);
    }
}
