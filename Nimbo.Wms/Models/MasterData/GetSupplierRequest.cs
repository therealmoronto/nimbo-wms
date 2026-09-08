using JetBrains.Annotations;
using Nimbo.Wms.Contracts.MasterData.Dtos;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record GetSupplierRequest(Guid SupplierId);

[PublicAPI]
public sealed record GetSupplierResponse(SupplierDto Value);
