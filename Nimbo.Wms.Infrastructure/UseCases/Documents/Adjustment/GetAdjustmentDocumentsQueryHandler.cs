using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Contracts.Documents.Adjustment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class GetAdjustmentDocumentsQueryHandler : IRequestHandler<GetAdjustmentDocumentsQuery, Result<List<AdjustmentDocumentBodyDto>>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<AdjustmentDocument, AdjustmentDocumentBodyDto> _mapper;

    public GetAdjustmentDocumentsQueryHandler(NimboWmsDbContext dbContext, IMapper<AdjustmentDocument, AdjustmentDocumentBodyDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<AdjustmentDocumentBodyDto>>> Handle(GetAdjustmentDocumentsQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<AdjustmentDocument>().AsNoTracking();
        var documents = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return documents;
    }
}
