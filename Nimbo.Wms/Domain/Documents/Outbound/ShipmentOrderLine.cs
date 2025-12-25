using Nimbo.Wms.Domain.Identification;
using Nimbo.Wms.Domain.References;

namespace Nimbo.Wms.Domain.Documents.Outbound;

public class ShipmentOrderLine
{
    /// <exception cref="ArgumentOutOfRangeException">Thrown when ordered quantity is not positive.</exception>
    public ShipmentOrderLine(
        Guid id,
        ItemId itemId,
        decimal orderedQuantity,
        UnitOfMeasure uomCode)
    {
        if (orderedQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderedQuantity));

        Id = id;
        ItemId = itemId;
        OrderedQuantity = orderedQuantity;
        UomCode = uomCode;

        ReservedQuantity = 0;
        PickedQuantity = 0;
    }

    public Guid Id { get; }

    public ItemId ItemId { get; }

    public decimal OrderedQuantity { get; }

    public UnitOfMeasure UomCode { get; }

    public decimal ReservedQuantity { get; private set; }

    public decimal PickedQuantity { get; private set; }

    public void Reserve(decimal qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));

        var newReserved = ReservedQuantity + qty;
        if (newReserved > OrderedQuantity)
            throw new InvalidOperationException("Cannot reserve more than ordered.");

        ReservedQuantity = newReserved;
    }

    public void Pick(decimal qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));

        var newPicked = PickedQuantity + qty;
        if (newPicked > ReservedQuantity)
            throw new InvalidOperationException("Cannot pick more than reserved.");

        PickedQuantity = newPicked;
    }
}
