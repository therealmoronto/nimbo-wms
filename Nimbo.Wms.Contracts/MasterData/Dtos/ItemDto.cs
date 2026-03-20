using JetBrains.Annotations;

namespace Nimbo.Wms.Contracts.MasterData.Dtos;

[PublicAPI]
public sealed record ItemDto(
    Guid Id,
    string Name,
    string InternalSku,
    string Barcode,
    string BaseUomCode,
    string? Manufacturer,
    decimal? WeightKg,
    decimal? VolumeM3
);
