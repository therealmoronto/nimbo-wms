using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record PatchItemCommand(
    Guid ItemGuid,
    string? Name = null,
    string? InternalSku = null,
    string? Barcode = null,
    string? BaseUom = null,
    bool? IsBatchManaged = null,
    string? Manufacturer = null,
    decimal? WeightKg = null,
    decimal? VolumeM3 = null
) : IRequest<Result>, ITxRequest;
