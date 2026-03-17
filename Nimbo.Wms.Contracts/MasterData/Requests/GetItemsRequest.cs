using MediatR;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record GetItemsRequest : IRequest<IReadOnlyList<ItemDto>>;
