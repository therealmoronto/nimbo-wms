using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Http;

public sealed record CreateItemRequest(
    string Name,
    string InternalSku,
    string Barcode,
    UnitOfMeasure BaseUom
);
