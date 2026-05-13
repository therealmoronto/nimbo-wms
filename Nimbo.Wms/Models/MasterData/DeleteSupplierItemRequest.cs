using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record DeleteSupplierItemRequest(Guid SupplierGuid, Guid SupplierItemIGuid);
