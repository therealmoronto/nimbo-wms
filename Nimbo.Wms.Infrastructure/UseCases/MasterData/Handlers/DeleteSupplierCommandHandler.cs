using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.MasterData.Commands;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Result>
{
    private readonly ISupplierRepository _repository;

    public DeleteSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteSupplierCommand command, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(command.SupplierGuid);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            return Error.NotFound("supplier.notfound", "Supplier not found");

        await _repository.DeleteAsync(supplier, ct);
        return Result.Success();
    }
}
