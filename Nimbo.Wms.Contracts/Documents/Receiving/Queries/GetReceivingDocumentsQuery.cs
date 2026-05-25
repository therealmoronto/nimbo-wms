using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Receiving.Queries;

[PublicAPI]
public record GetReceivingDocumentsQuery : IRequest<List<ReceivingDocumentBodyDto>>;
