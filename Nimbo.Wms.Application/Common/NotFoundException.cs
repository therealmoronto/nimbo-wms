namespace Nimbo.Wms.Application.Common;

public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message) { }
}
