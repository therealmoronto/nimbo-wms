using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Result<Guid>>
{
    private readonly IItemRepository _repository;

    public CreateItemCommandHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateItemCommand request, CancellationToken ct = default)
    {
        var item = new Item(
            ItemId.New(),
            request.Name,
            request.InternalSku,
            request.Barcode,
            Enum.Parse<UnitOfMeasure>(request.BaseUom),
            request.IsBatchManaged);

        await _repository.AddAsync(item, ct);

        return item.Id.Value;
    }
}
