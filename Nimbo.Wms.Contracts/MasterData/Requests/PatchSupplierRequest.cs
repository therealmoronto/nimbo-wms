using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

[PublicAPI]
public sealed record PatchSupplierRequest(
    Guid SupplierId,
    string? Code,
    string? Name,
    string? TaxId,
    string? Address,
    string? ContactName,
    string? Phone,
    string? Email,
    bool? IsActive
) : IRequest;
