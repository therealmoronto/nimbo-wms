using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Application.Mappings;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Queries;

public sealed class GetBatchesHandler : IQueryHandler<GetBatchesQuery, IReadOnlyList<BatchDto>>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Batch, BatchDto> _mapper;

    public GetBatchesHandler(NimboWmsDbContext dbContext, IMapper<Batch, BatchDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IReadOnlyList<BatchDto>> HandleAsync(GetBatchesQuery query, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Batch>().AsNoTracking();

        if (query.ItemId is not null)
            dbQuery = dbQuery.Where(b => b.ItemId == query.ItemId);

        if (query.SupplierId is not null)
            dbQuery = dbQuery.Where(b => b.SupplierId == query.SupplierId);

        var batches = await _mapper.ProjectToDto(dbQuery).ToListAsync(ct);

        return batches;
    }
}
