using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record CreateSupplierRequest(
    string Code,
    string Name
) : IRequest<SupplierId>, ITxRequest;

[PublicAPI]
public sealed record CreateSupplierResponse(Guid SupplierGuid);
