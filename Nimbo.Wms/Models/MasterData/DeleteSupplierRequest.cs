using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record DeleteSupplierRequest(Guid SupplierGuid);
