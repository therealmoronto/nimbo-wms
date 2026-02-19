using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;

public sealed record GetInventoryItemQuery(InventoryItemId InventoryItemId) : IQuery<InventoryItemDto>;
