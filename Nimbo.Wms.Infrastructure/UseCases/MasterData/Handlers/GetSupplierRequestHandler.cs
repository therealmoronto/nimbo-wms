using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;

public sealed class GetSupplierRequestHandler : IRequestHandler<GetSupplierRequest, SupplierDto>
{
    private readonly NimboWmsDbContext _dbContext;

    public GetSupplierRequestHandler(NimboWmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SupplierDto> Handle(GetSupplierRequest request, CancellationToken ct = default)
    {
        var supplier = await _dbContext.Set<Supplier>()
            .AsNoTracking()
            .Where(s => s.Id == request.SupplierId)
            .Select(s => new SupplierDto(s.Id.Value,
                s.Code,
                s.Name,
                s.TaxId,
                s.Address,
                s.ContactName,
                s.Phone,
                s.Email,
                s.IsActive,
                s.Items.Select(i => new SupplierItemDto(
                    i.Id.Value,
                    i.SupplierId.Value,
                    i.ItemId.Value,
                    i.SupplierSku,
                    i.SupplierBarcode,
                    i.DefaultPurchasePrice,
                    i.PurchaseUomCode,
                    i.UnitsPerPurchaseUom,
                    i.LeadTimeDays,
                    i.MinOrderQty,
                    i.IsPreferred)).ToList()
            ))
            .SingleOrDefaultAsync(ct);
        
        if (supplier == null)
            throw new NotFoundException($"Supplier not found.");

        return supplier;
    }
}
