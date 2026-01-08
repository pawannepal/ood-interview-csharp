namespace OodInterview.ShippingLocker.Account;

/// <summary>
/// Represents a user account in the locker system.
/// </summary>
public class Account
{
    private decimal _usageCharges;

    /// <summary>
    /// Creates a new account with specified details and policy.
    /// </summary>
    public Account(string accountId, string ownerName, AccountLockerPolicy lockerPolicy)
    {
        AccountId = accountId;
        OwnerName = ownerName;
        LockerPolicy = lockerPolicy;
        _usageCharges = 0.00m;
    }

    /// <summary>
    /// Gets the unique identifier of this account.
    /// </summary>
    public string AccountId { get; }

    /// <summary>
    /// Gets the name of the account owner.
    /// </summary>
    public string OwnerName { get; }

    /// <summary>
    /// Gets the locker policy associated with this account.
    /// </summary>
    public AccountLockerPolicy LockerPolicy { get; }

    /// <summary>
    /// Adds a charge to the account's total usage charges.
    /// </summary>
    public void AddUsageCharge(decimal amount)
    {
        _usageCharges += amount;
    }

    /// <summary>
    /// Gets the total usage charges for this account.
    /// </summary>
    public decimal UsageCharges => _usageCharges;
}
