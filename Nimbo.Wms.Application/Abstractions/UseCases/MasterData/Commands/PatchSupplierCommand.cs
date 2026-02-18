using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Http;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record PatchSupplierCommand(
    SupplierId SupplierId,
    PatchSupplierRequest Request
) : ICommand;

public sealed class PatchSupplierHandler : ICommandHandler<PatchSupplierCommand>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public PatchSupplierHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task HandleAsync(PatchSupplierCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        
        var supplierId = SupplierId.From(command.SupplierId);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");
        
        if (!string.IsNullOrWhiteSpace(request.Code))
            supplier.ChangeCode(request.Code);
        
        if (!string.IsNullOrWhiteSpace(request.Name))
            supplier.Rename(request.Name);
        
        if (!string.IsNullOrWhiteSpace(request.TaxId))
            supplier.ChangeTaxId(request.TaxId);
        
        if (!string.IsNullOrWhiteSpace(request.Address))
            supplier.ChangeAddress(request.Address);

        if (!string.IsNullOrWhiteSpace(request.ContactName) || !string.IsNullOrWhiteSpace(request.Phone) || !string.IsNullOrWhiteSpace(request.Email))
        {
            var contactName = request.ContactName ?? supplier.ContactName;
            var phone = request.Phone ?? supplier.Phone;
            var email = request.Email ?? supplier.Email;
            supplier.ChangeContact(contactName, phone, email);
        }

        if (request.IsActive.HasValue)
        {
            if (request.IsActive.Value)
                supplier.Activate();
            else
                supplier.Deactivate();
        }

        await _uow.SaveChangesAsync(ct);
    }
}
