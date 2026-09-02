using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record CreateItemRequest(
    string Name,
    string InternalSku,
    string Barcode,
    string BaseUom,
    bool IsBatchManaged = false
);

[PublicAPI]
public sealed record CreateItemResponse(Guid ItemGuid);
