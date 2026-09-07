using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record CreateItemCommand(
    string Name,
    string InternalSku,
    string Barcode,
    string BaseUom,
    bool IsBatchManaged = false
) : IRequest<Guid>, ITxRequest;
