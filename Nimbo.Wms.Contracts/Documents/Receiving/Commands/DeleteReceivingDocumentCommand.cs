using JetBrains.Annotations;
using MediatR;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Commands;

[PublicAPI]
public record DeleteReceivingDocumentCommand(Guid Id, long Version) : IRequest, ITxRequest;
