using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record DeleteSupplierCommand(SupplierId SupplierId) : ICommand;

public sealed class DeleteSupplierHandler : ICommandHandler<DeleteSupplierCommand>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteSupplierHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task HandleAsync(DeleteSupplierCommand command, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(command.SupplierId);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        await _repository.DeleteAsync(supplier, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
