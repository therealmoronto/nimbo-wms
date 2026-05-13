using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Contracts.Stock.Commands;

[PublicAPI]
public sealed record GetInventoryItemQuery(Guid InventoryItemId) : IRequest<InventoryItemDto>;
