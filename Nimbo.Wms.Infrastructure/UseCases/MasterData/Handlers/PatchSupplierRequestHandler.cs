using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class PatchSupplierRequestHandler : IRequestHandler<PatchSupplierRequest>
{
    private readonly ISupplierRepository _repository;

    public PatchSupplierRequestHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(PatchSupplierRequest request, CancellationToken ct = default)
    {
        var supplierId = SupplierId.From(request.SupplierId);
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
    }
}
