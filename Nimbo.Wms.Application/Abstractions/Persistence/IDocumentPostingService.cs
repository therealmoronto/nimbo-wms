using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Common;

namespace Nimbo.Wms.Application.Abstractions.Persistence;

[PublicAPI]
public interface IDocumentPostingService<in TDocument>
    where TDocument : IDocument
{
    Task PostAsync(TDocument document, CancellationToken ct = default);
}
