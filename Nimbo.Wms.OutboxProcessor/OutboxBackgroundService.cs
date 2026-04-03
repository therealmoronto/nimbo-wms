using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Infrastructure.Integrations;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Infrastructure.Persistence.Outbox;

namespace Nimbo.Wms.OutboxProcessor;

internal sealed class OutboxBackgroundService : BackgroundService
{
    public const int OutboxRetryCount = 5;

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxBackgroundService> _logger;
    private readonly IProducer<string, string> _producer;

    public OutboxBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<OutboxBackgroundService> logger,
        IProducer<string, string> producer)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessMessagesAsync(stoppingToken);
            }
            catch (KafkaException kafkaEx)
            {
                _logger.LogWarning("Kafka is not available: {Error}. Retrying in 30 seconds...", kafkaEx.Message);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in OutboxBackgroundService");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }

    private async Task ProcessMessagesAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<NimboWmsDbContext>();
        var erpService = scope.ServiceProvider.GetRequiredService<IErpIntegrationService>();

        var messages = await dbContext.Set<OutboxMessage>()
            .Where(x => !x.IsDeadLetter)
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.OccuredAt)
            .Take(20)
            .ToListAsync(stoppingToken);

        if (!messages.Any())
            return;

        foreach (var message in messages)
        {
            try
            {
                await erpService.NotifyEventAsync(message.Type, message.Content, stoppingToken);
                message.ProcessedAt = DateTime.UtcNow;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing message {MessageId}", message.Id);
                message.Error = e.Message;
                message.RetryCount++;
                if (message.RetryCount >= OutboxRetryCount)
                {
                    message.IsDeadLetter = true;
                    _logger.LogError(e, "Message {MessageId} is dead lettered", message.Id);
                }
            }
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}
