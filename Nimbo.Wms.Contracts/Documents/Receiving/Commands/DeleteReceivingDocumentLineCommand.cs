using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Common;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public sealed record DeleteReceivingDocumentLineCommand(
    Guid Id,
    long DocumentVersion
) : IRequest, ITxRequest;
