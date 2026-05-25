using JetBrains.Annotations;

namespace Nimbo.Wms.Models.Documents.Relocation;

[PublicAPI]
public sealed record AddRelocationDocumentLineResponse(Guid LineId);
