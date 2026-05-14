using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record PatchReceivingDocumentCommand(
    Guid Id,
    Guid? SupplierId,
    string? Code,
    string? Title,
    string? Notes,
    long Version
) : IRequest, ITxRequest;
