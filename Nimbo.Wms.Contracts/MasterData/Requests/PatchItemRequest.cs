using MediatR;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record PatchItemRequest(
    Guid ItemGuid,
    string? Name = null,
    string? InternalSku = null,
    string? Barcode = null,
    UnitOfMeasure? BaseUom = null,
    string? Manufacturer = null,
    decimal? WeightKg = null,
    decimal? VolumeM3 = null
) : IRequest;