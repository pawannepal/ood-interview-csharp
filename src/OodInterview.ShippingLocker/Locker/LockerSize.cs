namespace OodInterview.ShippingLocker.Locker;

/// <summary>
/// Enum representing different sizes of lockers with their dimensions and charges.
/// </summary>
public enum LockerSize
{
    /// <summary>Small locker with 10x10x10 dimensions and $5 daily charge.</summary>
    Small,
    /// <summary>Medium locker with 20x20x20 dimensions and $10 daily charge.</summary>
    Medium,
    /// <summary>Large locker with 30x30x30 dimensions and $15 daily charge.</summary>
    Large
}

/// <summary>
/// Extension methods for LockerSize to provide dimension and charge information.
/// </summary>
public static class LockerSizeExtensions
{
    public static string GetSizeName(this LockerSize size) => size switch
    {
        LockerSize.Small => "Small",
        LockerSize.Medium => "Medium",
        LockerSize.Large => "Large",
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    public static decimal GetDailyCharge(this LockerSize size) => size switch
    {
        LockerSize.Small => 5.00m,
        LockerSize.Medium => 10.00m,
        LockerSize.Large => 15.00m,
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    public static decimal GetWidth(this LockerSize size) => size switch
    {
        LockerSize.Small => 10.00m,
        LockerSize.Medium => 20.00m,
        LockerSize.Large => 30.00m,
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    public static decimal GetHeight(this LockerSize size) => size switch
    {
        LockerSize.Small => 10.00m,
        LockerSize.Medium => 20.00m,
        LockerSize.Large => 30.00m,
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };

    public static decimal GetDepth(this LockerSize size) => size switch
    {
        LockerSize.Small => 10.00m,
        LockerSize.Medium => 20.00m,
        LockerSize.Large => 30.00m,
        _ => throw new ArgumentOutOfRangeException(nameof(size))
    };
}
