using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Contracts.Documents.CycleCount.Queries;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

[PublicAPI]
public class GetCycleCountDocumentsQueryHandler : IRequestHandler<GetCycleCountDocumentsQuery, Result<IReadOnlyList<CycleCountDocumentBodyDto>>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<CycleCountDocument, CycleCountDocumentBodyDto> _mapper;

    public GetCycleCountDocumentsQueryHandler(
        NimboWmsDbContext dbContext,
        IMapper<CycleCountDocument, CycleCountDocumentBodyDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<CycleCountDocumentBodyDto>>> Handle(
        GetCycleCountDocumentsQuery request,
        CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<CycleCountDocument>().AsNoTracking();
        var documents = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return documents.AsReadOnly();
    }
}
