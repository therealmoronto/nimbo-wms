using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Contracts.Documents.Relocation.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class GetRelocationDocumentLinesQueryHandler : IRequestHandler<GetRelocationDocumentLinesQuery, Result<List<RelocationDocumentLineDto>>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<RelocationDocumentLine, RelocationDocumentLineDto> _mapper;

    public GetRelocationDocumentLinesQueryHandler(NimboWmsDbContext dbContext, IMapper<RelocationDocumentLine, RelocationDocumentLineDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<RelocationDocumentLineDto>>> Handle(GetRelocationDocumentLinesQuery request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.Id);
        var dbQuery = _dbContext.Set<RelocationDocumentLine>()
            .AsNoTracking()
            .Where(x => x.DocumentId == documentId);

        var lines = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return lines;
    }
}
