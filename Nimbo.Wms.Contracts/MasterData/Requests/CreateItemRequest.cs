using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record CreateItemRequest(
    string Name,
    string InternalSku,
    string Barcode,
    string BaseUom
) : IRequest<Guid>, ITxRequest;

[PublicAPI]
public sealed record CreateItemResponse(Guid ItemGuid);
