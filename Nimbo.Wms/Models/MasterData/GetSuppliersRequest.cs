using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record GetSuppliersRequest;

[PublicAPI]
public sealed record GetSuppliersResponse(IReadOnlyList<SupplierDto> Value);

