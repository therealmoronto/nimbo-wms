using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record AddSupplierItemRequest(Guid SupplierGuid, Guid ItemGuid) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record AddSupplierItemResponse(Guid SupplierItemGuid);
