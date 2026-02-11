using Xunit.Sdk;

namespace Nimbo.Wms.Tests.Common;

public static class TestSkip
{
    /// <summary>
    /// Check if a test should be skipped.
    /// </summary>
    /// <param name="condition">Skip condition</param>
    /// <param name="reason">Reason for skipping the test</param>
    /// <exception cref="SkipException">Thrown when test should be skipped</exception>
    public static void If(bool condition, string reason)
    {
        if (condition)
            throw SkipException.ForSkip(reason);
    }
}
