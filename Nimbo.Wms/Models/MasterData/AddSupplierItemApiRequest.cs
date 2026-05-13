using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record AddSupplierItemRequest(Guid SupplierGuid, Guid ItemGuid);

[PublicAPI]
public sealed record AddSupplierItemResponse(Guid SupplierItemGuid);
