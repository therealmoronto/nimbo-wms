using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Guid>
{
    private readonly IWarehouseRepository _repository;

    public CreateWarehouseCommandHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateWarehouseCommand command, CancellationToken ct = default)
    {
        var id = WarehouseId.New();
        var warehouse = new Warehouse(id, command.Code, command.Name);
        await _repository.AddAsync(warehouse, ct);

        return id;
    }
}
