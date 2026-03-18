using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record AddSupplierItemRequest(Guid SupplierGuid, Guid ItemGuid) : IRequest<SupplierItemId>;

[PublicAPI]
public sealed record AddSupplierItemResponse(Guid SupplierItemGuid);
