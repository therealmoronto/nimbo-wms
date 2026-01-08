namespace Nimbo.Wms.Domain.References;

public enum TransferOrderStatus
{
    Draft = 0,
    Picking = 10,
    InTransit = 20,
    ReceivedPartially = 30,
    ReceivedFully = 40,
    Cancelled = 99
}
