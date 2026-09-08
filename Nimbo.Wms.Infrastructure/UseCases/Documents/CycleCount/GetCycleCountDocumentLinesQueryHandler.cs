using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Contracts.Documents.CycleCount.Queries;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class GetCycleCountDocumentLinesQueryHandler : IRequestHandler<GetCycleCountDocumentLinesQuery, Result<IReadOnlyList<CycleCountDocumentLineDto>>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<CycleCountDocumentLine, CycleCountDocumentLineDto> _mapper;

    public GetCycleCountDocumentLinesQueryHandler(
        NimboWmsDbContext dbContext,
        IMapper<CycleCountDocumentLine, CycleCountDocumentLineDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<CycleCountDocumentLineDto>>> Handle(
        GetCycleCountDocumentLinesQuery request,
        CancellationToken cancellationToken)
    {
        var documentId = CycleCountDocumentId.From(request.DocumentId);
        var dbQuery = _dbContext.Set<CycleCountDocumentLine>()
            .AsNoTracking()
            .Where(l => l.DocumentId == documentId);

        var lines = await _mapper.ProjectToDto(dbQuery).ToListAsync(cancellationToken);
        return lines.AsReadOnly();
    }
}
