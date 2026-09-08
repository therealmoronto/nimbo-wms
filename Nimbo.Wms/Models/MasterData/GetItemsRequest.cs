using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record GetItemsRequest;

[PublicAPI]
public sealed record GetItemsResponse(IReadOnlyList<ItemDto> Value);
