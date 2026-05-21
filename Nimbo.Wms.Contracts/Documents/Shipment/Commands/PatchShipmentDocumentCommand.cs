using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Shipment.Commands;

[PublicAPI]
public sealed record PatchShipmentDocumentCommand(
    Guid Id,
    Guid? CustomerId,
    string? Code,
    string? Title,
    string? Notes,
    long Version
) : IRequest, ITxRequest;
