namespace OodInterview.ShippingLocker.Package;

/// <summary>
/// Exception thrown when a package cannot fit in any available locker size.
/// </summary>
public class PackageIncompatibleException : Exception
{
    public PackageIncompatibleException(string message) : base(message)
    {
    }
}
