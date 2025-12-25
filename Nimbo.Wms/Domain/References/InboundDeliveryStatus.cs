namespace Nimbo.Wms.Domain.References;

public enum InboundDeliveryStatus
{
    Draft = 0,
    InProgress = 10,
    ReceivedPartially = 20,
    ReceivedFully = 30,
    Cancelled = 99,
}
