using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record PatchItemCommand(
    Guid ItemGuid,
    string? Name = null,
    string? InternalSku = null,
    string? Barcode = null,
    string? BaseUom = null,
    string? Manufacturer = null,
    decimal? WeightKg = null,
    decimal? VolumeM3 = null
) : IRequest, ITxRequest;
