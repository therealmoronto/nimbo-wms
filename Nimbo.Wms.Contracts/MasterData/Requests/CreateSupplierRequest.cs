using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record CreateSupplierRequest(
    string Code,
    string Name
) : IRequest<SupplierId>;

public sealed record CreateSupplierResponse(Guid SupplierGuid);
