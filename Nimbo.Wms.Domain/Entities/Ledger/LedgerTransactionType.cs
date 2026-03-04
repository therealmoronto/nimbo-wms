using JetBrains.Annotations;
using Nimbo.Wms.Domain.Entities.Documents.Adjustment;
using Nimbo.Wms.Domain.Entities.Documents.CycleCount;
using Nimbo.Wms.Domain.Entities.Documents.Receiving;
using Nimbo.Wms.Domain.Entities.Documents.Relocation;
using Nimbo.Wms.Domain.Entities.Documents.Shipment;

namespace Nimbo.Wms.Domain.Entities.Ledger;

/// <summary>
/// Represents the type of transaction that is recorded in the ledger.
/// </summary>
[PublicAPI]
public enum LedgerTransactionType
{
    /// <summary>
    /// Represents a receipt of inventory on Post() of <see cref="ReceivingDocument"/>.
    /// </summary>
    Receipt,

    /// <summary>
    /// Represents a receipt of inventory on Post() of <see cref="ShipmentDocument"/>.
    /// </summary>
    Shipment,

    /// <summary>
    /// Represents a transfer of inventory <bold>to location</bold> on Post() of <see cref="RelocationDocument"/>.
    /// </summary>
    TransferIn,

    /// <summary>
    /// Represents a transfer of inventory <bold>from location</bold> on Post() of <see cref="RelocationDocument"/>.
    /// </summary>
    TransferOut,

    /// <summary>
    /// Represents an adjustment made automatically by the system on Post() of <see cref="CycleCountDocument"/>.
    /// </summary>
    CountingAdjustment,

    /// <summary>
    /// Represents an adjustment made manually by a user on Post() of <see cref="AdjustmentDocument"/>.
    /// </summary>
    ManualAdjustment,
}
