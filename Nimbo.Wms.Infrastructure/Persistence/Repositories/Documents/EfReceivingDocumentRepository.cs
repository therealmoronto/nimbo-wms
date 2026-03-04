using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

public sealed class EfReceivingDocumentRepository : EfDocumentRepository<ReceivingDocument, ReceivingDocumentId>, IReceivingDocumentRepository
{
    public EfReceivingDocumentRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override async Task<ReceivingDocument?> GetByIdAsync(ReceivingDocumentId id, CancellationToken ct = default)
    {
        return await Set.Include(d => d.Lines)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
