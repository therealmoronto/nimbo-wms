using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Movements;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Movements;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Topology;

namespace Nimbo.Wms.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IWarehouseRepository, EfWarehouseRepository>();
        
        services.AddScoped<IItemRepository, EfItemRepository>();
        services.AddScoped<ICustomerRepository, EfCustomerRepository>();
        services.AddScoped<ISupplierRepository, EfSupplierRepository>();

        services.AddScoped<IBatchRepository, EfBatchRepository>();
        services.AddScoped<IInventoryItemRepository, EfInventoryItemRepository>();

        services.AddScoped<IInternalTransferRepository, EfInternalTransferRepository>();

        services.AddScoped<IInboundDeliveryRepository, EfInboundDeliveryRepository>();
        services.AddScoped<IShipmentOrderRepository, EfShipmentOrderRepository>();
        services.AddScoped<ITransferOrderRepository, EfTransferOrderRepository>();
        services.AddScoped<IInventoryCountRepository, EfInventoryCountRepository>();

        return services;
    }
}
