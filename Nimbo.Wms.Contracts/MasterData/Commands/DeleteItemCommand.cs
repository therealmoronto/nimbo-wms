using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Commands;

[PublicAPI]
public sealed record DeleteItemCommand(Guid ItemGuid) : IRequest<Result>, ITxRequest;
