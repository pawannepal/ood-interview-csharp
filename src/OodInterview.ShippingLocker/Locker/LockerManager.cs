using OodInterview.ShippingLocker.Package;

namespace OodInterview.ShippingLocker.Locker;

/// <summary>
/// Manages the operations of lockers at a site.
/// </summary>
public class LockerManager
{
    private readonly Site _site;
    private readonly INotificationService _notificationService;
    private readonly Dictionary<string, Account.Account> _accounts;
    private readonly Dictionary<string, Locker> _accessCodeMap = [];

    /// <summary>
    /// Creates a new locker manager for a site.
    /// </summary>
    public LockerManager(Site site, Dictionary<string, Account.Account> accounts, INotificationService notificationService)
    {
        _site = site;
        _accounts = accounts;
        _notificationService = notificationService;
    }

    /// <summary>
    /// Assigns a package to an available locker.
    /// </summary>
    public Locker AssignPackage(IShippingPackage pkg, DateTime date)
    {
        var locker = _site.PlacePackage(pkg, date);
        if (locker.AccessCode != null)
        {
            _accessCodeMap[locker.AccessCode] = locker;
            _notificationService.SendNotification($"Package assigned to locker {locker.AccessCode}", pkg.User);
        }
        return locker;
    }

    /// <summary>
    /// Processes package pickup using an access code.
    /// </summary>
    public Locker? PickUpPackage(string accessCode)
    {
        if (!_accessCodeMap.TryGetValue(accessCode, out var locker))
        {
            return null;
        }

        if (!locker.CheckAccessCode(accessCode))
        {
            return null;
        }

        try
        {
            var charge = locker.CalculateStorageCharges();
            var pkg = locker.Package;
            locker.ReleaseLocker();
            if (pkg != null)
            {
                pkg.User.AddUsageCharge(charge);
                pkg.UpdateShippingStatus(ShippingStatus.Retrieved);
            }
            return locker;
        }
        catch (MaximumStoragePeriodExceededException)
        {
            locker.ReleaseLocker();
            return locker;
        }
    }

    /// <summary>
    /// Returns an account by its ID.
    /// </summary>
    public Account.Account? GetAccount(string accountId)
    {
        return _accounts.GetValueOrDefault(accountId);
    }
}
