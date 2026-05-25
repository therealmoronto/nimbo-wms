using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Contracts.Documents.Receiving.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class GetReceivingDocumentLinesQueryHandler : IRequestHandler<GetReceivingDocumentLinesQuery, List<ReceivingDocumentLineDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto> _mapper;

    public GetReceivingDocumentLinesQueryHandler(NimboWmsDbContext dbContext, IMapper<ReceivingDocumentLine, ReceivingDocumentLineDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ReceivingDocumentLineDto>> Handle(GetReceivingDocumentLinesQuery request, CancellationToken cancellationToken)
    {
        var documentId = ReceivingDocumentId.From(request.Id);
        var dbQuery = _dbContext.Set<ReceivingDocumentLine>()
            .AsNoTracking()
            .Where(l => l.DocumentId == documentId);

        var lines = await _mapper.ProjectToDto(dbQuery).ToListAsync(cancellationToken);
        return lines;
    }
}
