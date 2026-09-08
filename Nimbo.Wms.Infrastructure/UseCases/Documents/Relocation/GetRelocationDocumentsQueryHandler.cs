using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Contracts.Documents.Relocation.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class GetRelocationDocumentsQueryHandler : IRequestHandler<GetRelocationDocumentsQuery, Result<List<RelocationDocumentBodyDto>>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<RelocationDocument, RelocationDocumentBodyDto> _mapper;

    public GetRelocationDocumentsQueryHandler(NimboWmsDbContext dbContext, IMapper<RelocationDocument, RelocationDocumentBodyDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<RelocationDocumentBodyDto>>> Handle(GetRelocationDocumentsQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<RelocationDocument>().AsNoTracking();
        var documents = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return documents;
    }
}
