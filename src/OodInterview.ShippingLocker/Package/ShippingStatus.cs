namespace OodInterview.ShippingLocker.Package;

/// <summary>
/// Represents the shipping status of a package.
/// </summary>
public enum ShippingStatus
{
    /// <summary>Initial state when package is first created.</summary>
    Created,
    /// <summary>Package is ready for locker assignment.</summary>
    Pending,
    /// <summary>Package has been assigned to a locker.</summary>
    InLocker,
    /// <summary>Package has been picked up by the customer.</summary>
    Retrieved,
    /// <summary>Package has exceeded maximum storage time.</summary>
    Expired
}
