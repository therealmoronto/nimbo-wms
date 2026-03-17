using MediatR;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Contracts.MasterData.Requests;

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
