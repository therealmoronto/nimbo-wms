using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.Stock.Requests;

[PublicAPI]
public sealed record GetInventoryItemRequest(InventoryItemId InventoryItemId) : IRequest<InventoryItemDto>;
