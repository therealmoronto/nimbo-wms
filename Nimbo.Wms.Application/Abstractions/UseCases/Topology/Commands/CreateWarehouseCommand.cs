using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record CreateWarehouseCommand(string Code, string Name) : ICommand<WarehouseId>;

public sealed class CreateWarehouseHandler : ICommandHandler<CreateWarehouseCommand, WarehouseId>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWarehouseHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<WarehouseId> HandleAsync(CreateWarehouseCommand command, CancellationToken ct = default)
    {
        var id = WarehouseId.New();

        var warehouse = new Warehouse(id, command.Code, command.Name);

        await _repository.AddAsync(warehouse, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return id;
    }
}
