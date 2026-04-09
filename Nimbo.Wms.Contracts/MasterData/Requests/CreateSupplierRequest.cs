using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record CreateSupplierRequest(
    string Code,
    string Name
) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record CreateSupplierResponse(Guid SupplierGuid);
