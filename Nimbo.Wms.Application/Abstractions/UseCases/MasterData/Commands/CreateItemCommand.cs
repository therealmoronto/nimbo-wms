using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record CreateItemCommand(
    CreateItemRequest Request
) : ICommand<ItemId>;


public sealed class CreateItemHandler : ICommandHandler<CreateItemCommand, ItemId>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreateItemHandler(IItemRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }
    
    public async Task<ItemId> HandleAsync(CreateItemCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        var item = new Item(
            ItemId.New(),
            request.Name,
            request.InternalSku,
            request.Barcode,
            request.BaseUom);

        await _repository.AddAsync(item, ct);
        await _uow.SaveChangesAsync(ct);

        return item.Id;
    }
}
