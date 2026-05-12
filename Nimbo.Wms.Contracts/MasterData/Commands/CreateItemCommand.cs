using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record CreateItemCommand(
    string Name,
    string InternalSku,
    string Barcode,
    string BaseUom
) : IRequest<Guid>, ITxRequest;
