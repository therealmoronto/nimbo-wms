namespace Nimbo.Wms.Domain.References;

public enum InboundDeliveryStatus
{
    Draft = 0,
    Cancelled = 5,
    ReceivedPartially = 10,
    ReceivedFully = 20,
}
