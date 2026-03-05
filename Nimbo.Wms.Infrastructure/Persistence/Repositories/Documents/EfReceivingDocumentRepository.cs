using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Identification;

namespace Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;

[PublicAPI]
internal sealed class EfReceivingDocumentRepository : EfDocumentRepository<ReceivingDocument, ReceivingDocumentId>, IReceivingDocumentRepository
{
    public EfReceivingDocumentRepository(NimboWmsDbContext dbContext)
        : base(dbContext) { }

    public override Task<ReceivingDocument?> GetByIdAsync(ReceivingDocumentId id, CancellationToken ct = default)
    {
        return Set.Include(d => d.Lines)
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }
}
