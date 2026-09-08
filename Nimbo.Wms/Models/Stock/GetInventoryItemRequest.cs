using JetBrains.Annotations;
using Nimbo.Wms.Contracts.Stock.Dtos;

namespace Nimbo.Wms.Models.Stock;

[PublicAPI]
public sealed record GetInventoryItemRequest(Guid InventoryItemId);

[PublicAPI]
public sealed record GetInventoryItemResponse(InventoryItemDto Value);
