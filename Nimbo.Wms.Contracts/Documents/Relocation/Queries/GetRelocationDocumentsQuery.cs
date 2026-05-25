using JetBrains.Annotations;
using MediatR;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;

namespace Nimbo.Wms.Contracts.Documents.Relocation.Queries;

[PublicAPI]
public record GetRelocationDocumentsQuery : IRequest<List<RelocationDocumentBodyDto>>;
