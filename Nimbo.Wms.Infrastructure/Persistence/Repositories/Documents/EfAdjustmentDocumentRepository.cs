using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

[PublicAPI]
internal sealed class EfAdjustmentDocumentRepository : EfDocumentRepository<AdjustmentDocument, AdjustmentDocumentId>, IAdjustmentDocumentRepository
{
    public EfAdjustmentDocumentRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<AdjustmentDocument?> GetByIdAsync(AdjustmentDocumentId id, CancellationToken ct = default)
    {
        return Set.Include(d => d.Lines)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
