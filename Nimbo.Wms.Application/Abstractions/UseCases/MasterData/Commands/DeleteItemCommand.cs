using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record DeleteItemCommand(ItemId ItemId) : ICommand;

public sealed class DeleteItemHandler : ICommandHandler<DeleteItemCommand>
{
    private readonly IItemRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteItemHandler(IItemRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }
    
    public async Task HandleAsync(DeleteItemCommand command, CancellationToken ct = default)
    {
        var item = await _repository.GetByIdAsync(command.ItemId, ct);
        if (item is null)
            throw new NotFoundException($"Item with id {command.ItemId} not found");

        await _repository.DeleteAsync(item, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
