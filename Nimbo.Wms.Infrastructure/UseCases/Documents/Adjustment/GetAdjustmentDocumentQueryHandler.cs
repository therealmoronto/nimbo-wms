using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Adjustment.Dtos;
using Nimbo.Wms.Contracts.Documents.Adjustment.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Adjustment;

public class GetAdjustmentDocumentQueryHandler : IRequestHandler<GetAdjustmentDocumentQuery, AdjustmentDocumentDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<AdjustmentDocument, AdjustmentDocumentBodyDto> _bodyMapper;
    private readonly IMapper<AdjustmentDocumentLine, AdjustmentDocumentLineDto> _lineMapper;

    public GetAdjustmentDocumentQueryHandler(
        NimboWmsDbContext dbContext,
        IMapper<AdjustmentDocument, AdjustmentDocumentBodyDto> bodyMapper,
        IMapper<AdjustmentDocumentLine, AdjustmentDocumentLineDto> lineMapper)
    {
        _dbContext = dbContext;
        _bodyMapper = bodyMapper;
        _lineMapper = lineMapper;
    }

    public async Task<AdjustmentDocumentDto> Handle(GetAdjustmentDocumentQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<AdjustmentDocument>()
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .Include(d => d.Lines);

        var document = await dbQuery.Select(d =>
                new AdjustmentDocumentDto(
                    _bodyMapper.MapToDto(d),
                    _lineMapper.MapToDto(d.Lines).ToList()))
            .SingleOrDefaultAsync(ct);
            
        if (document is null)
            throw new NotFoundException($"Adjustment document with ID {request.Id} not found");

        return document;
    }
}
