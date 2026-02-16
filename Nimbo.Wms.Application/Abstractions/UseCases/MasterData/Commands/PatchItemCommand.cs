using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Http;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record PatchItemCommand(PatchItemRequest Request) : ICommand;

public sealed class PatchItemHandler : ICommandHandler<PatchItemCommand>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _uow;

    public PatchItemHandler(IItemRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }
    
    public async Task HandleAsync(PatchItemCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        
        var item = await _repository.GetByIdAsync(request.ItemId, ct);
        if (item is null)
            throw new NotFoundException($"Item with id {request.ItemId} not found");
        
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

        await _uow.SaveChangesAsync(ct);
    }
}
