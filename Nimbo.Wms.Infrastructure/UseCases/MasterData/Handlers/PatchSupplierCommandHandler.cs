using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class PatchSupplierCommandHandler : IRequestHandler<PatchSupplierCommand>
{
    private readonly ISupplierRepository _repository;

    public PatchSupplierCommandHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchSupplierCommand command, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(command.SupplierId);
        var supplier = await _repository.GetByIdAsync(supplierId, ct);
        if (supplier is null)
            throw new NotFoundException("Supplier not found");
        
        if (!string.IsNullOrWhiteSpace(command.Code))
            supplier.ChangeCode(command.Code);
        
        if (!string.IsNullOrWhiteSpace(command.Name))
            supplier.Rename(command.Name);
        
        if (!string.IsNullOrWhiteSpace(command.TaxId))
            supplier.ChangeTaxId(command.TaxId);
        
        if (!string.IsNullOrWhiteSpace(command.Address))
            supplier.ChangeAddress(command.Address);

        if (!string.IsNullOrWhiteSpace(command.ContactName) || !string.IsNullOrWhiteSpace(command.Phone) || !string.IsNullOrWhiteSpace(command.Email))
        {
            var contactName = command.ContactName ?? supplier.ContactName;
            var phone = command.Phone ?? supplier.Phone;
            var email = command.Email ?? supplier.Email;
            supplier.ChangeContact(contactName, phone, email);
        }

        if (command.IsActive.HasValue)
        {
            if (command.IsActive.Value)
                supplier.Activate();
            else
                supplier.Deactivate();
        }
    }
}
