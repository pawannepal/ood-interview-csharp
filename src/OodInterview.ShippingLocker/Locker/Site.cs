using OodInterview.ShippingLocker.Package;

namespace OodInterview.ShippingLocker.Locker;

/// <summary>
/// Represents a physical site containing multiple lockers of different sizes.
/// </summary>
public class Site
{
    private readonly Dictionary<LockerSize, HashSet<Locker>> _lockers = [];

    /// <summary>
    /// Creates a new site with specified number of lockers for each size.
    /// </summary>
    public Site(Dictionary<LockerSize, int> lockers)
    {
        foreach (var (size, count) in lockers)
        {
            var lockerSet = new HashSet<Locker>();
            for (var i = 0; i < count; i++)
            {
                lockerSet.Add(new Locker(size));
            }
            _lockers[size] = lockerSet;
        }
    }

    /// <summary>
    /// Finds an available locker of the specified size.
    /// </summary>
    public Locker? FindAvailableLocker(LockerSize size)
    {
        if (!_lockers.TryGetValue(size, out var lockers))
        {
            return null;
        }

        foreach (var locker in lockers)
        {
            if (locker.IsAvailable)
            {
                return locker;
            }
        }
        return null;
    }

    /// <summary>
    /// Places a package in an available locker of appropriate size.
    /// </summary>
    public Locker PlacePackage(IShippingPackage pkg, DateTime date)
    {
        // Determine the smallest locker size that can fit this package
        var size = pkg.GetLockerSize();
        var locker = FindAvailableLocker(size);
        if (locker != null)
        {
            locker.AssignPackage(pkg, date);
            pkg.UpdateShippingStatus(ShippingStatus.InLocker);
            return locker;
        }
        throw new NoLockerAvailableException($"No locker of size {size} is currently available");
    }
}
