using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record PatchShipmentDocumentCommand(
    Guid Id,
    Guid? CustomerId,
    string? Code,
    string? Title,
    string? Notes,
    long Version
) : IRequest<Result>, ITxRequest;
