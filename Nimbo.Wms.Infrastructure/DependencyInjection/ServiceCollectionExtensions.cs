using Confluent.Kafka;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Nimbo.Wms.Application;
using Nimbo.Wms.Application.Abstractions.Persistence;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Documents;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Ledger;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.MasterData;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Stock;
using Nimbo.Wms.Application.Abstractions.Persistence.Repositories.Topology;
using Nimbo.Wms.Application.Common.Behaviors;
using Nimbo.Wms.Application.Mappings.MasterData;
using Nimbo.Wms.Application.Mappings.Stock;
using Nimbo.Wms.Application.Mappings.Topology;
using Nimbo.Wms.Application.Services.Documents;
using Nimbo.Wms.Contracts.Common;
using Nimbo.Wms.Contracts.MasterData.Dtos;
using Nimbo.Wms.Contracts.Stock.Dtos;
using Nimbo.Wms.Contracts.Topology.Dtos;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;
using Nimbo.Wms.Domain.Entities.MasterData;
using Nimbo.Wms.Domain.Entities.Stock;
using Nimbo.Wms.Domain.Entities.Topology;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Documents;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Ledger;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.MasterData;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Stock;
using Nimbo.Wms.Infrastructure.Persistence.Repositories.Topology;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;

namespace Nimbo.Wms.Infrastructure.DependencyInjection;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddResiliencePipeline("kafka-cb", builder =>
            {
                var options = new CircuitBreakerStrategyOptions
                {
                    ShouldHandle = new PredicateBuilder().Handle<ProduceException<string, string>>(),
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(30),
                    BreakDuration = TimeSpan.FromSeconds(30),
                };

                builder.AddCircuitBreaker(options);
            });

            services.AddTopology();
            services.AddMasterData();
            services.AddStock();
            services.AddDocuments();

            services.AddValidatorsFromAssembly(typeof(IApplicationMarker).Assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
            });

            return services;
        }

        private IServiceCollection AddTopology()
        {
            services.AddScoped<IMapper<Warehouse, WarehouseTopologyDto>, WarehouseTopologyMapper>();
            services.AddScoped<IMapper<Warehouse, WarehouseListItemDto>, WarehouseListItemMapper>();
            services.AddScoped<IMapper<Zone, ZoneDto>, ZoneMapper>();
            services.AddScoped<IMapper<Location, LocationDto>, LocationMapper>();

            services.AddScoped<IWarehouseRepository, EfWarehouseRepository>();
            return services;
        }

        private IServiceCollection AddMasterData()
        {
            services.AddScoped<IMapper<Item, ItemDto>, ItemMapper>();
            services.AddScoped<IMapper<Supplier, SupplierDto>, SupplierMapper>();
            services.AddScoped<IMapper<SupplierItem, SupplierItemDto>, SupplierItemMapper>();

            services.AddScoped<IItemRepository, EfItemRepository>();
            services.AddScoped<ICustomerRepository, EfCustomerRepository>();
            services.AddScoped<ISupplierRepository, EfSupplierRepository>();
            return services;
        }

        private IServiceCollection AddStock()
        {
            services.AddScoped<IMapper<Batch, BatchDto>, BatchMapper>();
            services.AddScoped<IMapper<InventoryItem, InventoryItemDto>, InventoryItemMapper>();

            services.AddScoped<IBatchRepository, EfBatchRepository>();
            services.AddScoped<IInventoryItemRepository, EfInventoryItemRepository>();
            return services;
        }

        private IServiceCollection AddDocuments()
        {
            services.AddScoped<IDocumentPostingService<ReceivingDocument>, ReceivingDocumentPostingService>();
            services.AddScoped<IDocumentPostingService<RelocationDocument>, RelocationDocumentPostingService>();
            services.AddScoped<IDocumentPostingService<ShipmentDocument>, ShipmentDocumentPostingService>();
            services.AddScoped<IDocumentPostingService<AdjustmentDocument>, AdjustmentDocumentPostingService>();
            services.AddScoped<IDocumentPostingService<CycleCountDocument>, CycleCountDocumentPostingService>();

            services.AddScoped<IReceivingDocumentRepository, EfReceivingDocumentRepository>();
            services.AddScoped<IRelocationDocumentRepository, EfRelocationDocumentRepository>();
            services.AddScoped<IShipmentDocumentRepository, EfShipmentDocumentRepository>();
            services.AddScoped<IAdjustmentDocumentRepository, EfAdjustmentDocumentRepository>();
            services.AddScoped<ICycleCountDocumentRepository, EfCycleCountDocumentRepository>();

            services.AddScoped<IStockLedgerEntryRepository, EfStockLedgerEntryRepository>();

            return services;
        }

        private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var random = new Random();
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(random.Next(0, 100)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
