using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class PatchItemCommandHandler : IRequestHandler<PatchItemCommand, Result>
{
    private readonly IItemRepository _repository;

    public PatchItemCommandHandler(IItemRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(PatchItemCommand command, CancellationToken ct = default)
    {
        var itemId = ItemId.From(command.ItemGuid);
        var item = await _repository.GetByIdAsync(itemId, ct);
        if (item is null)
            return Error.NotFound("item.notfound", $"Item with id {itemId} not found");
        
        if (!string.IsNullOrWhiteSpace(command.Name))
            item.Rename(command.Name);
        
        if (!string.IsNullOrWhiteSpace(command.InternalSku))
            item.ChangeInternalSku(command.InternalSku);
        
        if (!string.IsNullOrWhiteSpace(command.Barcode))
            item.ChangeBarcode(command.Barcode);

        if (!string.IsNullOrEmpty(command.BaseUom) && Enum.TryParse(command.BaseUom, out UnitOfMeasure baseUom))
            item.ChangeBaseUom(baseUom);

        if (command.IsBatchManaged is not null)
            item.ChangeIsBatchManaged(command.IsBatchManaged.Value);

        if (!string.IsNullOrWhiteSpace(command.Manufacturer))
            item.ChangeManufacturer(command.Manufacturer);

        if (command.WeightKg is not null || command.VolumeM3 is not null)
        {
            var weight = command.WeightKg ?? item.WeightKg;
            var volume = command.VolumeM3 ?? item.VolumeM3;
            item.SetPhysical(weight, volume);
        }

        return Result.Success();
    }
}
