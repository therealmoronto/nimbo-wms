using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.CycleCount.Dtos;
using Nimbo.Wms.Contracts.Documents.CycleCount.Queries;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.CycleCount;

public class GetCycleCountDocumentQueryHandler : IRequestHandler<GetCycleCountDocumentQuery, Result<CycleCountDocumentDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<CycleCountDocument, CycleCountDocumentBodyDto> _bodyMapper;
    private readonly IMapper<CycleCountDocumentLine, CycleCountDocumentLineDto> _lineMapper;

    public GetCycleCountDocumentQueryHandler(
        NimboWmsDbContext dbContext,
        IMapper<CycleCountDocument, CycleCountDocumentBodyDto> bodyMapper,
        IMapper<CycleCountDocumentLine, CycleCountDocumentLineDto> lineMapper)
    {
        _dbContext = dbContext;
        _bodyMapper = bodyMapper;
        _lineMapper = lineMapper;
    }

    public async Task<Result<CycleCountDocumentDto>> Handle(GetCycleCountDocumentQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<CycleCountDocument>()
            .AsNoTracking()
            .Where(d => d.Id == request.DocumentId)
            .Include(d => d.Lines);

        var document = await dbQuery.Select(d =>
                new CycleCountDocumentDto(
                    _bodyMapper.MapToDto(d),
                    _lineMapper.MapToDto(d.Lines).ToList()))
            .SingleOrDefaultAsync(ct);

        if (document is null)
            return Error.NotFound("document.notfound", $"Cycle count document with ID {request.DocumentId} not found");

        return document;
    }
}
