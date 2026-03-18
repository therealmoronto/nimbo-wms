using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;

public sealed record GetInventoryItemRequest(InventoryItemId InventoryItemId) : IRequest<InventoryItemDto>;
