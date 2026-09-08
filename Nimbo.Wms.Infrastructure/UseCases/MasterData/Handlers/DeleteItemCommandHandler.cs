using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, Result>
{
    private readonly IItemRepository _repository;

    public DeleteItemCommandHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteItemCommand command, CancellationToken ct = default)
    {
        var itemId = ItemId.From(command.ItemGuid);
        var item = await _repository.GetByIdAsync(itemId, ct);
        if (item is null)
        {
            return Error.NotFound("item.notfound", $"Item with id {itemId} not found");
        }

        await _repository.DeleteAsync(item, ct);
        return Result.Success();
    }
}
