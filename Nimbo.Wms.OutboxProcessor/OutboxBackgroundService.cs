using System.Text.Json;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Nimbo.Wms.Domain;
using Nimbo.Wms.Infrastructure.Persistence;
using Nimbo.Wms.Infrastructure.Persistence.Outbox;
using Polly;
using Polly.Registry;

namespace Nimbo.Wms.OutboxProcessor;

internal sealed class OutboxBackgroundService : BackgroundService
{
    public const int OutboxRetryCount = 5;

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxBackgroundService> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly ResiliencePipeline _pipeline;

    public OutboxBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<OutboxBackgroundService> logger,
        IProducer<string, string> producer,
        ResiliencePipelineProvider<string> pipelineProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _producer = producer;
        _pipeline = pipelineProvider.GetPipeline("kafka-cb");
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
                var domainEventId = JsonSerializer.Deserialize<IDomainEvent>(message.Content);
                if (domainEventId is null)
                    continue;

                var kafkaMessage = new Message<string, string>
                {
                    Key = domainEventId.AggregateId.ToString(),
                    Value = message.Content,
                    Timestamp = new Timestamp(DateTime.UtcNow),
                };

                var state = new { _producer, kafkaMessage };
                var deliveryResult = await _pipeline.ExecuteAsync(
                    static async (stateArgs, ct) => await stateArgs._producer.ProduceAsync("outbox-topic", stateArgs.kafkaMessage, ct),
                    state,
                    stoppingToken);

                message.ProcessedAt = DateTime.UtcNow;
                if (_logger.IsEnabled(LogLevel.Debug))
                    _logger.LogDebug(
                        "Message {MessageId} delivered to {Topic} at offset {Offset}",
                        message.Id,
                        deliveryResult.Topic,
                        deliveryResult.Offset);
            }
            catch (ProduceException<string, string> e)
            {
                _logger.LogError(e, "Kafka delivery failed for message {MessageId}. Reason: {Reason}", message.Id, e.Error.Reason);

                message.Error = e.Error.Reason;
                message.RetryCount++;

                if (message.RetryCount >= OutboxRetryCount)
                {
                    message.IsDeadLetter = true;
                    _logger.LogError(e, "Message {MessageId} is dead lettered", message.Id);
                }
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
