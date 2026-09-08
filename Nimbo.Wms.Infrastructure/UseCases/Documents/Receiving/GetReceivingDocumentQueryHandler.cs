using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Contracts.Documents.Receiving.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

public class GetReceivingDocumentQueryHandler : IRequestHandler<GetReceivingDocumentQuery, Result<ReceivingDocumentDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ReceivingDocument, ReceivingDocumentBodyDto> _bodyMapper;
    private readonly IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto> _lineMapper;

    public GetReceivingDocumentQueryHandler(
        NimboWmsDbContext dbContext,
        IMapper<ReceivingDocument, ReceivingDocumentBodyDto> bodyMapper,
        IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto> lineMapper)
    {
        _dbContext = dbContext;
        _bodyMapper = bodyMapper;
        _lineMapper = lineMapper;
    }

    public async Task<Result<ReceivingDocumentDto>> Handle(GetReceivingDocumentQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<ReceivingDocument>()
            .AsNoTracking()
            .Where(d => d.Id == request.Id)
            .Include(d => d.Lines);

        var document = await dbQuery.Select(d =>
                new ReceivingDocumentDto(
                    _bodyMapper.MapToDto(d),
                    _lineMapper.MapToDto(d.Lines).ToList()))
            .SingleOrDefaultAsync(ct);
        if (document is null)
            return Error.NotFound("document.notfound", $"Receiving document with ID {request.Id} not found");

        return document;
    }
}
