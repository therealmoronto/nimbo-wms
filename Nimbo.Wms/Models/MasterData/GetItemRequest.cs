using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record GetItemRequest(Guid ItemId);

[PublicAPI]
public sealed record GetItemResponse(ItemDto Value);
