using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record CreateItemRequest(
    string Name,
    string InternalSku,
    string Barcode,
    UnitOfMeasure BaseUom
) : IRequest<ItemId>, ITxRequest;

[PublicAPI]
public sealed record CreateItemResponse(Guid ItemGuid);
