using MediatR;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record CreateItemRequest(
    string Name,
    string InternalSku,
    string Barcode,
    UnitOfMeasure BaseUom
) : IRequest<ItemId>;

public sealed record CreateItemResponse(Guid ItemGuid);
