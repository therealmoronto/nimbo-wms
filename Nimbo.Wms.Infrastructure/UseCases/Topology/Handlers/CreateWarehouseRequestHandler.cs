using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class CreateWarehouseRequestHandler : IRequestHandler<CreateWarehouseRequest, Guid>
{
    private readonly IWarehouseRepository _repository;

    public CreateWarehouseRequestHandler(IWarehouseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateWarehouseRequest request, CancellationToken ct = default)
    {
        var id = WarehouseId.New();
        var warehouse = new Warehouse(id, request.Code, request.Name);
        await _repository.AddAsync(warehouse, ct);

        return id;
    }
}
