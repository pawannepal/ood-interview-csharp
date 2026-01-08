namespace OodInterview.ShippingLocker.Package;

/// <summary>
/// Exception thrown when a package exceeds its maximum allowed storage period.
/// </summary>
public class MaximumStoragePeriodExceededException : Exception
{
    public MaximumStoragePeriodExceededException(string message) : base(message)
    {
    }
}
