using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class DeleteSupplierRequestHandler : IRequestHandler<DeleteSupplierRequest>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public DeleteSupplierRequestHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task Handle(DeleteSupplierRequest request, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(request.SupplierGuid);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");

        await _repository.DeleteAsync(supplier, ct);
        await _uow.CommitAsync(ct);
    }
}
