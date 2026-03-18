using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

[PublicAPI]
internal sealed class GetBatchesRequestHandler : IRequestHandler<GetBatchesRequest, IReadOnlyList<BatchDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Batch, BatchDto> _mapper;

    public GetBatchesRequestHandler(NimboWmsDbContext dbContext, IMapper<Batch, BatchDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<BatchDto>> Handle(GetBatchesRequest request, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Batch>().AsNoTracking();

        if (request.ItemId is not null)
            dbQuery = dbQuery.Where(b => b.ItemId == request.ItemId);

        if (request.SupplierId is not null)
            dbQuery = dbQuery.Where(b => b.SupplierId == request.SupplierId);

        var batches = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);

        return batches;
    }
}
