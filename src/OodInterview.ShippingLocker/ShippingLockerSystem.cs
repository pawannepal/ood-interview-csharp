using OodInterview.ShippingLocker.Locker;
using OodInterview.ShippingLocker.Package;

namespace OodInterview.ShippingLocker;

/// <summary>
/// Main facade class for the shipping locker system.
/// </summary>
public class ShippingLockerSystem
{
    private readonly LockerManager _lockerManager;

    /// <summary>
    /// Creates a new shipping locker system.
    /// </summary>
    public ShippingLockerSystem(LockerManager lockerManager)
    {
        _lockerManager = lockerManager;
    }

    /// <summary>
    /// Creates a shipping locker system with specified site configuration.
    /// </summary>
    public static ShippingLockerSystem Create(
        Dictionary<LockerSize, int> lockerConfiguration,
        Dictionary<string, Account.Account> accounts,
        INotificationService notificationService)
    {
        var site = new Site(lockerConfiguration);
        var lockerManager = new LockerManager(site, accounts, notificationService);
        return new ShippingLockerSystem(lockerManager);
    }

    /// <summary>
    /// Assigns a package to an available locker.
    /// </summary>
    public Locker.Locker AssignPackage(IShippingPackage package, DateTime deliveryDate)
    {
        return _lockerManager.AssignPackage(package, deliveryDate);
    }

    /// <summary>
    /// Picks up a package using an access code.
    /// </summary>
    public Locker.Locker? PickUpPackage(string accessCode)
    {
        return _lockerManager.PickUpPackage(accessCode);
    }

    /// <summary>
    /// Gets an account by its ID.
    /// </summary>
    public Account.Account? GetAccount(string accountId)
    {
        return _lockerManager.GetAccount(accountId);
    }
}
