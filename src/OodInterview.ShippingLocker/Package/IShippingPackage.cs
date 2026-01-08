using OodInterview.ShippingLocker.Locker;

namespace OodInterview.ShippingLocker.Package;

/// <summary>
/// Interface defining the contract for shipping packages.
/// </summary>
public interface IShippingPackage
{
    /// <summary>
    /// Gets the unique order identifier.
    /// </summary>
    string OrderId { get; }

    /// <summary>
    /// Gets the user account associated with this package.
    /// </summary>
    Account.Account User { get; }

    /// <summary>
    /// Gets the width of the package.
    /// </summary>
    decimal Width { get; }

    /// <summary>
    /// Gets the height of the package.
    /// </summary>
    decimal Height { get; }

    /// <summary>
    /// Gets the depth of the package.
    /// </summary>
    decimal Depth { get; }

    /// <summary>
    /// Gets the current status of the package.
    /// </summary>
    ShippingStatus Status { get; }

    /// <summary>
    /// Updates the status of the package.
    /// </summary>
    void UpdateShippingStatus(ShippingStatus status);

    /// <summary>
    /// Determines the appropriate locker size for this package.
    /// </summary>
    LockerSize GetLockerSize();
}
