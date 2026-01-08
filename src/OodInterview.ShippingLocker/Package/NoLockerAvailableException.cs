namespace OodInterview.ShippingLocker.Package;

/// <summary>
/// Exception thrown when no locker of the required size is available.
/// </summary>
public class NoLockerAvailableException : Exception
{
    public NoLockerAvailableException(string message) : base(message)
    {
    }
}
