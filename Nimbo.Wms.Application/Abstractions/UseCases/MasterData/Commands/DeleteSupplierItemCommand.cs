using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record DeleteSupplierItemCommand(SupplierId SupplierId, SupplierItemId SupplierItemId) : ICommand;

public sealed class DeleteSupplierItemHandler : ICommandHandler<DeleteSupplierItemCommand>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteSupplierItemHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task HandleAsync(DeleteSupplierItemCommand command, CancellationToken ct = default)
    {
        var supplier = await _repository.GetByIdAsync(command.SupplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        if (!supplier.RemoveItem(command.SupplierItemId))
            throw new NotFoundException("Supplier item not found");

        await _uow.SaveChangesAsync(ct);
    }
}
