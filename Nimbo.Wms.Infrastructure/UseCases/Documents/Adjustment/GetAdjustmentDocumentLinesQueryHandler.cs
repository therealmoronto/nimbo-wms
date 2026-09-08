using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Contracts.Documents.Adjustment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

[PublicAPI]
public class GetAdjustmentDocumentLinesQueryHandler : IRequestHandler<GetAdjustmentDocumentLinesQuery, Result<List<AdjustmentDocumentLineDto>>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<AdjustmentDocumentLine, AdjustmentDocumentLineDto> _mapper;

    public GetAdjustmentDocumentLinesQueryHandler(NimboWmsDbContext dbContext, IMapper<AdjustmentDocumentLine, AdjustmentDocumentLineDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<AdjustmentDocumentLineDto>>> Handle(GetAdjustmentDocumentLinesQuery request, CancellationToken cancellationToken)
    {
        var documentId = AdjustmentDocumentId.From(request.Id);
        var dbQuery = _dbContext.Set<AdjustmentDocumentLine>()
            .AsNoTracking()
            .Where(l => l.DocumentId == documentId);

        var lines = await _mapper.ProjectToDto(dbQuery).ToListAsync(cancellationToken);
        return lines;
    }
}
