using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class CreateItemRequestHandler : IRequestHandler<CreateItemRequest, Guid>
{
    private readonly IItemRepository _repository;

    public CreateItemRequestHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateItemRequest request, CancellationToken ct = default)
    {
        var item = new Item(
            ItemId.New(),
            request.Name,
            request.InternalSku,
            request.Barcode,
            Enum.Parse<UnitOfMeasure>(request.BaseUom));

        await _repository.AddAsync(item, ct);

        return item.Id;
    }
}
