using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Application.Common;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.MasterData.Requests;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Infrastructure.Persistence;

namespace Nimbo.Wms.Infrastructure.UseCases.MasterData.Handlers;

[PublicAPI]
internal sealed class GetSupplierRequestHandler : IRequestHandler<GetSupplierRequest, SupplierDto>
{
    private readonly NimboWmsDbContext _dbContext;
    private readonly IMapper<Supplier, SupplierDto> _mapper;

    public GetSupplierRequestHandler(NimboWmsDbContext dbContext, IMapper<Supplier, SupplierDto> mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SupplierDto> Handle(GetSupplierRequest request, CancellationToken ct = default)
    {
        var dbQuery = _dbContext.Set<Supplier>()
            .AsNoTracking()
            .Where(s => s.Id == request.SupplierId);

        var supplier = await _mapper.ProjectToDto(dbQuery).SingleOrDefaultAsync(ct);
        if (supplier == null)
            throw new NotFoundException($"Supplier not found.");

        return supplier;
    }
}
