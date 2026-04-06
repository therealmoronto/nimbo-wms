using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class CreateSupplierRequestHandler : IRequestHandler<CreateSupplierRequest, SupplierId>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreateSupplierRequestHandler(ISupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<SupplierId> Handle(CreateSupplierRequest request, CancellationToken ct = default)
    {
        var supplerId = SupplierId.From(Guid.NewGuid());
        var supplier = new Supplier(supplerId, request.Code, request.Name);
        
        await _repository.AddAsync(supplier, ct);

        return supplerId;
    }
}
