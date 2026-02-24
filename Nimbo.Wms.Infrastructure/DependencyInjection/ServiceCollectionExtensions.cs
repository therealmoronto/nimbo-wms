using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application.Abstractions.Cqrs;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.MasterData.Queries;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.Stock.Queries;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Commands;
using Nimbo.Wms.Application.Abstractions.UseCases.Topology.Queries;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Topology;
using Nimbo.Wms.Infrastructure.UseCases.MasterData.Queries;
using Nimbo.Wms.Infrastructure.UseCases.Stock.Queries;
using Nimbo.Wms.Infrastructure.UseCases.Topology.Queries;

namespace Nimbo.Wms.Infrastructure.DependencyInjection;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddTopology();
            services.AddMasterData();
            services.AddStock();

            return services;
        }

        private IServiceCollection AddTopology()
        {
            services.AddScoped<IWarehouseRepository, EfWarehouseRepository>();
        
            services.AddScoped<ICommandHandler<CreateWarehouseCommand, WarehouseId>, CreateWarehouseHandler>();
            services.AddScoped<ICommandHandler<AddZoneToWarehouseCommand, ZoneId>, AddZoneToWarehouseHandler>();
            services.AddScoped<ICommandHandler<AddLocationToWarehouseCommand, LocationId>, AddLocationToWarehouseHandler>();

            services.AddScoped<IQueryHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>>, GetWarehousesHandler>();
            services.AddScoped<IQueryHandler<GetWarehouseTopologyQuery, WarehouseTopologyDto>, GetWarehouseTopologyHandler>();

            services.AddScoped<ICommandHandler<PatchLocationCommand>, PatchLocationHandler>();
            services.AddScoped<ICommandHandler<PatchZoneCommand>, PatchZoneHandler>();
            services.AddScoped<ICommandHandler<PatchWarehouseCommand>, PatchWarehouseHandler>();

            services.AddScoped<ICommandHandler<DeleteLocationCommand>, DeleteLocationHandler>();
            services.AddScoped<ICommandHandler<DeleteZoneCommand>, DeleteZoneHandler>();
            services.AddScoped<ICommandHandler<DeleteWarehouseCommand>, DeleteWarehouseHandler>();

            return services;
        }

        private IServiceCollection AddMasterData()
        {
            services.AddScoped<IItemRepository, EfItemRepository>();
            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<ISupplierRepository, EfSupplierRepository>();

            services.AddScoped<ICommandHandler<CreateItemCommand, ItemId>, CreateItemHandler>();
            services.AddScoped<ICommandHandler<PatchItemCommand>, PatchItemHandler>();
            services.AddScoped<ICommandHandler<DeleteItemCommand>, DeleteItemHandler>();

            services.AddScoped<IQueryHandler<GetItemQuery, ItemDto>, GetItemHandler>();
            services.AddScoped<IQueryHandler<GetItemsQuery, IReadOnlyList<ItemDto>>, GetItemsHandler>();

            services.AddScoped<ICommandHandler<CreateSupplierCommand, SupplierId>, CreateSupplierHandler>();
            services.AddScoped<ICommandHandler<PatchSupplierCommand>, PatchSupplierHandler>();
            services.AddScoped<ICommandHandler<DeleteSupplierCommand>, DeleteSupplierHandler>();
            services.AddScoped<ICommandHandler<AddSupplierItemCommand, SupplierItemId>, AddSupplierItemHandler>();
            services.AddScoped<ICommandHandler<PatchSupplierItemCommand>, PatchSupplierItemHandler>();
            services.AddScoped<ICommandHandler<DeleteSupplierItemCommand>, DeleteSupplierItemHandler>();

            services.AddScoped<IQueryHandler<GetSupplierQuery, SupplierDto>, GetSupplierHandler>();
            services.AddScoped<IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierDto>>, GetSuppliersHandler>();

            return services;
        }

        private IServiceCollection AddStock()
        {
            services.AddScoped<IBatchRepository, EfBatchRepository>();
            services.AddScoped<IInventoryItemRepository, EfInventoryItemRepository>();
        
            services.AddScoped<ICommandHandler<CreateBatchCommand, BatchId>, CreateBatchHandler>();
            services.AddScoped<ICommandHandler<CreateInventoryItemCommand, InventoryItemId>, CreateInventoryItemHandler>();

            services.AddScoped<IQueryHandler<GetInventoryItemQuery, InventoryItemDto>, GetInventoryItemHandler>();
            services.AddScoped<IQueryHandler<GetBatchQuery, BatchDto>, GetBatchHandler>();
            services.AddScoped<IQueryHandler<GetInventoryItemsQuery, IReadOnlyList<InventoryItemDto>>, GetInventoryItemsHandler>();
            services.AddScoped<IQueryHandler<GetBatchesQuery, IReadOnlyList<BatchDto>>, GetBatchesHandler>();

            return services;
        }
    }
}
