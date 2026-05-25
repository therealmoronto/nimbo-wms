using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Relocation.Dtos;
using Nimbo.Wms.Contracts.Documents.Relocation.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Relocation;

[PublicAPI]
public class GetRelocationDocumentQueryHandler : IRequestHandler<GetRelocationDocumentQuery, RelocationDocumentDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<RelocationDocument, RelocationDocumentBodyDto> _documentMapper;
    private readonly IMapper<RelocationDocumentLine, RelocationDocumentLineDto> _lineMapper;

    public GetRelocationDocumentQueryHandler(NimboWmsDbContext dbContext, IMapper<RelocationDocument, RelocationDocumentBodyDto> documentMapper, IMapper<RelocationDocumentLine, RelocationDocumentLineDto> lineMapper)
    {
        _dbContext = dbContext;
        _documentMapper = documentMapper;
        _lineMapper = lineMapper;
    }

    public async Task<RelocationDocumentDto> Handle(GetRelocationDocumentQuery request, CancellationToken ct)
    {
        var documentId = RelocationDocumentId.From(request.Id);
        
        var document = await _dbContext.Set<RelocationDocument>()
            .AsNoTracking()
            .Include(x => x.Lines)
            .FirstOrDefaultAsync(x => x.Id == documentId, ct);

        if (document == null)
            throw new NotFoundException($"Relocation document with ID '{request.Id}' not found.");

        var bodyDto = _documentMapper.MapToDto(document);
        var linesDto = _lineMapper.MapToDto(document.Lines).ToList();

        return new RelocationDocumentDto(bodyDto, linesDto);
    }
}
