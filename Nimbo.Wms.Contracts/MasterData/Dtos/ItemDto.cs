using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Dtos;

public sealed record ItemDto(
    Guid Id,
    string Name,
    string InternalSku,
    string Barcode,
    UnitOfMeasure BaseUom,
    string? Manufacturer,
    decimal? WeightKg,
    decimal? VolumeM3
);
