using MassTransit;
using Nimbo.Wms.Domain.Entities.Documents;

namespace Nimbo.Wms.DummyConsumer;

public class DummyConsumer(ILogger<DummyConsumer> logger) : IConsumer<IDocumentPostedEvent>
{
    public Task Consume(ConsumeContext<IDocumentPostedEvent> context)
    {
        var evt = context.Message;
        logger.LogInformation("[Kafka] Received document posted event: {EvtId}", evt.Id);
        return Task.CompletedTask;
    }
}
