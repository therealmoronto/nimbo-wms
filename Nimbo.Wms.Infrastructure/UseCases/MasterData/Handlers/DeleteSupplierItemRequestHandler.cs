using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class DeleteSupplierItemRequestHandler : IRequestHandler<DeleteSupplierItemRequest>
{
    private readonly ISupplierRepository _repository;

    public DeleteSupplierItemRequestHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteSupplierItemRequest request, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(request.SupplierGuid);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        var supplierItemId = SupplierItemId.From(request.SupplierItemIGuid);
        if (!supplier.RemoveItem(supplierItemId))
            throw new NotFoundException("Supplier item not found");
    }
}
