using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Contracts.MasterData.Queries;

[PublicAPI]
public sealed record GetItemQuery(Guid ItemId) : IRequest<ItemDto>;
