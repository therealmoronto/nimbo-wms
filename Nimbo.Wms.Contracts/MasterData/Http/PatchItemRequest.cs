using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Http;

public sealed record PatchItemRequest(
    string? Name = null,
    string? InternalSku = null,
    string? Barcode = null,
    UnitOfMeasure? BaseUom = null,
    string? Manufacturer = null,
    decimal? WeightKg = null,
    decimal? VolumeM3 = null
);
