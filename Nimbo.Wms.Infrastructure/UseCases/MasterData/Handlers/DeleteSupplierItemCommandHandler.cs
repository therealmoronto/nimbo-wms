using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class DeleteSupplierItemCommandHandler : IRequestHandler<DeleteSupplierItemCommand, Result>
{
    private readonly ISupplierRepository _repository;

    public DeleteSupplierItemCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteSupplierItemCommand command, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(command.SupplierGuid);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            return Error.NotFound("supplier.notfound", "Supplier not found");

        var supplierItemId = SupplierItemId.From(command.SupplierItemIGuid);
        if (!supplier.RemoveItem(supplierItemId))
            return Error.NotFound("supplier_item.notfound", "Supplier item not found");

        return Result.Success();
    }
}
