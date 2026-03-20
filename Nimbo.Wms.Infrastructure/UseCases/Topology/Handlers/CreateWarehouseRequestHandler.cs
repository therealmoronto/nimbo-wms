using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Contracts.Topology.Requests;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.Topology.Handlers;

[PublicAPI]
internal sealed class CreateWarehouseRequestHandler : IRequestHandler<CreateWarehouseRequest, WarehouseId>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseRequestHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WarehouseId> Handle(CreateWarehouseRequest request, CancellationToken ct = default)
    {
        var id = WarehouseId.New();
        var warehouse = new Warehouse(id, request.Code, request.Name);
        await _repository.AddAsync(warehouse, ct);
        await _unitOfWork.CommitAsync(ct);
        return id;
    }
}
