namespace OodInterview.GroceryStore.Discount.Strategy;

/// <summary>
/// Strategy that applies a percentage-based discount.
/// </summary>
public class PercentageBasedStrategy : IDiscountCalculationStrategy
{
    private readonly decimal _percentage;

    /// <summary>
    /// Creates a new percentage-based strategy with the specified percentage.
    /// </summary>
    /// <param name="percentage">The percentage to be discounted (e.g., 50 for 50%).</param>
    public PercentageBasedStrategy(decimal percentage)
    {
        _percentage = percentage;
    }

    /// <inheritdoc />
    public decimal CalculateDiscountedPrice(decimal originalPrice)
    {
        return originalPrice * (1 - _percentage / 100);
    }
}
