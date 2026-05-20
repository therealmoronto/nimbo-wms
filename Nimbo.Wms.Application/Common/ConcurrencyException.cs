namespace Nimbo.Wms.Application.Common;

public class ConcurrencyException : Exception
{
    public ConcurrencyException(string message)
        : base(message) { }
}
