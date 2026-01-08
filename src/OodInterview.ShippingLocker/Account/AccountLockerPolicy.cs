namespace OodInterview.ShippingLocker.Account;

/// <summary>
/// Defines the policy for locker usage for an account.
/// </summary>
public class AccountLockerPolicy
{
    /// <summary>
    /// Creates a new locker policy with specified free and maximum periods.
    /// </summary>
    /// <param name="freePeriodDays">Number of days of free storage.</param>
    /// <param name="maximumPeriodDays">Maximum number of days a package can be stored.</param>
    public AccountLockerPolicy(int freePeriodDays, int maximumPeriodDays)
    {
        FreePeriodDays = freePeriodDays;
        MaximumPeriodDays = maximumPeriodDays;
    }

    /// <summary>
    /// Gets the number of free storage days.
    /// </summary>
    public int FreePeriodDays { get; }

    /// <summary>
    /// Gets the maximum allowed storage period in days.
    /// </summary>
    public int MaximumPeriodDays { get; }
}
