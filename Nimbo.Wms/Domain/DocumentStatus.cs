using JetBrains.Annotations;

namespace Nimbo.Wms.Domain;

/// <summary>
/// Document statuses for <see cref="IDocument"/>
/// For more information see https://github.com/therealmoronto/nimbo-wms/wiki/processes-overview
/// </summary>
[PublicAPI]
public enum DocumentStatus
{
    Draft = 1,
    InProgress,
    Completed,
    Cancelled,
}
