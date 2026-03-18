using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Stock.Requests;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.Stock.Handlers;

public sealed class GetBatchesRequestHandler : IRequestHandler<GetBatchesRequest, IReadOnlyList<BatchDto>>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetBatchesRequestHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyList<BatchDto>> Handle(GetBatchesRequest request, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Batch>().AsNoTracking();

        if (request.ItemId is not null)
            dbQuery = dbQuery.Where(b => b.ItemId == request.ItemId);

        if (request.SupplierId is not null)
            dbQuery = dbQuery.Where(b => b.SupplierId == request.SupplierId);

        var batches = await dbQuery.Select(b => new BatchDto(
                b.Id,
                b.ItemId,
                b.BatchNumber,
                b.SupplierId,
                b.ManufacturedAt,
                b.ExpiryDate,
                b.ReceivedAt,
                b.Notes))
            .ToListAsync(ct);

        return batches;
    }
}
