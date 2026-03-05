using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

[PublicAPI]
internal sealed class EfRelocationDocumentRepository : EfDocumentRepository<RelocationDocument, RelocationDocumentId>, IRelocationDocumentRepository
{
    public EfRelocationDocumentRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<RelocationDocument?> GetByIdAsync(RelocationDocumentId id, CancellationToken ct = default)
    {
        return Set.Include(d => d.Lines)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
