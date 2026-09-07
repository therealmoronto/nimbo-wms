using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts;
using Nimbo.Wms.Contracts.Documents.Receiving.Dtos;
using Nimbo.Wms.Contracts.Documents.Receiving.Queries;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Documents.Receiving;

[PublicAPI]
public class GetReceivingDocumentsQueryHandler : IRequestHandler<GetReceivingDocumentsQuery, List<ReceivingDocumentBodyDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<ReceivingDocument, ReceivingDocumentBodyDto> _mapper;

    public GetReceivingDocumentsQueryHandler(NimboWmsDbContext dbContext, IMapper<ReceivingDocument, ReceivingDocumentBodyDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ReceivingDocumentBodyDto>> Handle(GetReceivingDocumentsQuery request, CancellationToken ct)
    {
        var dbQuery = _dbContext.Set<ReceivingDocument>().AsNoTracking();
        var documents = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);
        return documents;
    }
}
