using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record AddSupplierItemRequest(Guid SupplierGuid, Guid ItemGuid) : IRequest<SupplierItemId>;

public sealed record AddSupplierItemResponse(Guid SupplierItemGuid);
