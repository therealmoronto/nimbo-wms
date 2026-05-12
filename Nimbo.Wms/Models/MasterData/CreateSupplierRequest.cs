using JetBrains.Annotations;

namespace Nimbo.Wms.Models.MasterData;

[PublicAPI]
public sealed record CreateSupplierRequest(
    string Code,
    string Name
);

[PublicAPI]
public sealed record CreateSupplierResponse(Guid SupplierGuid);
