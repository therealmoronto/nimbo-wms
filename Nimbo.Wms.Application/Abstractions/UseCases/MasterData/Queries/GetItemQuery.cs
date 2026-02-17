using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;

public sealed record GetItemQuery(ItemId ItemId) : IQuery<ItemDto>;
