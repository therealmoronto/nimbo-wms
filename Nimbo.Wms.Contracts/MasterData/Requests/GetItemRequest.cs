using MediatR;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

public sealed record GetItemRequest(ItemId ItemId) : IRequest<ItemDto>;
