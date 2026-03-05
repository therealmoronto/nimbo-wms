using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

public class EfCycleCountDocumentRepository : EfDocumentRepository<CycleCountDocument, CycleCountDocumentId>, ICycleCountDocumentRepository
{
    public EfCycleCountDocumentRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<CycleCountDocument?> GetByIdAsync(CycleCountDocumentId id, CancellationToken ct = default)
    {
        return Set.Include(d => d.Lines)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
