using OodInterview.ShippingLocker.Account;
using OodInterview.ShippingLocker.Package;

namespace OodInterview.ShippingLocker.Locker;

/// <summary>
/// Represents a physical locker that can store packages.
/// </summary>
public class Locker
{
    private static readonly Random Random = new();

    private IShippingPackage? _currentPackage;
    private DateTime? _assignmentDate;
    private string? _accessCode;

    /// <summary>
    /// Creates a new locker of the specified size.
    /// </summary>
    public Locker(LockerSize size)
    {
        Size = size;
    }

    /// <summary>
    /// Gets the size of this locker.
    /// </summary>
    public LockerSize Size { get; }

    /// <summary>
    /// Assigns a package to this locker and generates an access code.
    /// </summary>
    public void AssignPackage(IShippingPackage pkg, DateTime date)
    {
        _currentPackage = pkg;
        _assignmentDate = date;
        _accessCode = GenerateAccessCode();
    }

    /// <summary>
    /// Releases the locker by removing the current package and its details.
    /// </summary>
    public void ReleaseLocker()
    {
        _currentPackage = null;
        _assignmentDate = null;
        _accessCode = null;
    }

    /// <summary>
    /// Calculates storage charges based on usage duration and policy.
    /// </summary>
    public decimal CalculateStorageCharges()
    {
        if (_currentPackage == null || _assignmentDate == null)
        {
            return 0m;
        }

        var policy = _currentPackage.User.LockerPolicy;
        var totalDaysUsed = (long)(DateTime.Now - _assignmentDate.Value).TotalDays;

        // Check if exceeds maximum period
        if (totalDaysUsed > policy.MaximumPeriodDays)
        {
            _currentPackage.UpdateShippingStatus(ShippingStatus.Expired);
            throw new MaximumStoragePeriodExceededException(
                $"Package has exceeded maximum allowed storage period of {policy.MaximumPeriodDays} days");
        }

        // Calculate chargeable days (excluding free period)
        var chargeableDays = Math.Max(0, totalDaysUsed - policy.FreePeriodDays);
        return Size.GetDailyCharge() * chargeableDays;
    }

    /// <summary>
    /// Checks if the locker is available for new packages.
    /// </summary>
    public bool IsAvailable => _currentPackage == null;

    /// <summary>
    /// Verifies if the provided access code matches the locker's code.
    /// </summary>
    public bool CheckAccessCode(string code)
    {
        return _accessCode != null && _accessCode == code;
    }

    /// <summary>
    /// Generates a random 6-digit access code.
    /// </summary>
    private static string GenerateAccessCode()
    {
        var code = 100000 + Random.Next(900000);
        return code.ToString();
    }

    /// <summary>
    /// Gets the current access code.
    /// </summary>
    public string? AccessCode => _accessCode;

    /// <summary>
    /// Gets the currently stored package.
    /// </summary>
    public IShippingPackage? Package => _currentPackage;
}
