using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record PatchItemRequest(
    Guid ItemGuid,
    string? Name = null,
    string? InternalSku = null,
    string? Barcode = null,
    string? BaseUom = null,
    string? Manufacturer = null,
    decimal? WeightKg = null,
    decimal? VolumeM3 = null
);
