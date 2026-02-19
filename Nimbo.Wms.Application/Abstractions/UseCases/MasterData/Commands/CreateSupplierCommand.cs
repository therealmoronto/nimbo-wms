using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Contracts.Topology.Http;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;

public sealed record CreateSupplierCommand(CreateSupplierRequest Request) : ICommand<SupplierId>;

public sealed class CreateSupplierHandler : ICommandHandler<CreateSupplierCommand, SupplierId>
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _uow;

    public CreateSupplierHandler(ISupplierRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<SupplierId> HandleAsync(CreateSupplierCommand command, CancellationToken ct = default)
    {
        var request = command.Request;
        var supplerId = SupplierId.From(Guid.NewGuid());
        var supplier = new Supplier(supplerId, request.Code, request.Name);
        
        await _repository.AddAsync(supplier, ct);
        await _uow.SaveChangesAsync(ct);

        return supplerId;
    }
}
