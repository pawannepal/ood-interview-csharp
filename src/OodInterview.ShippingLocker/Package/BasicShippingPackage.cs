using OodInterview.ShippingLocker.Locker;

namespace OodInterview.ShippingLocker.Package;

/// <summary>
/// Basic implementation of a shipping package.
/// </summary>
public class BasicShippingPackage : IShippingPackage
{
    /// <summary>
    /// Creates a new shipping package with specified dimensions.
    /// </summary>
    public BasicShippingPackage(string orderId, Account.Account user, decimal width, decimal height, decimal depth)
    {
        OrderId = orderId;
        User = user;
        Width = width;
        Height = height;
        Depth = depth;
        Status = ShippingStatus.Created;
    }

    public string OrderId { get; }
    public Account.Account User { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public decimal Depth { get; }
    public ShippingStatus Status { get; private set; }

    public void UpdateShippingStatus(ShippingStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Determines the smallest locker size that can fit this package.
    /// </summary>
    public LockerSize GetLockerSize()
    {
        foreach (var size in Enum.GetValues<LockerSize>())
        {
            if (size.GetWidth() >= Width &&
                size.GetHeight() >= Height &&
                size.GetDepth() >= Depth)
            {
                return size;
            }
        }
        throw new PackageIncompatibleException("No locker size available for the package");
    }
}
