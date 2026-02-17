using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;

public sealed record PatchWarehouseCommand(
    WarehouseId WarehouseId,
    PatchWarehouseRequest PatchWarehouseRequest
) : ICommand;

public sealed class PatchWarehouseHandler : ICommandHandler<PatchWarehouseCommand>
{
    private readonly IWarehouseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PatchWarehouseHandler(IWarehouseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }


    public async Task HandleAsync(PatchWarehouseCommand command, CancellationToken ct = default)
    {
        var warehouse = await _repository.GetByIdAsync(command.WarehouseId, ct);
        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        var request = command.PatchWarehouseRequest;

        warehouse.Rename(request.Name ?? warehouse.Name);
        warehouse.ChangeCode(request.Code ?? warehouse.Code);
        warehouse.ChangeAddress(request.Address ?? warehouse.Address);
        warehouse.ChangeDescription(request.Description ?? warehouse.Description);

        await _unitOfWork.SaveChangesAsync(ct);
    }
}
