namespace OodInterview.GroceryStore.Discount.Strategy;

/// <summary>
/// Strategy that applies a fixed amount discount.
/// </summary>
public class AmountBasedStrategy : IDiscountCalculationStrategy
{
    private readonly decimal _discountAmount;

    /// <summary>
    /// Creates a new amount-based strategy with the specified discount amount.
    /// </summary>
    /// <param name="discountAmount">The fixed amount to be discounted.</param>
    public AmountBasedStrategy(decimal discountAmount)
    {
        _discountAmount = discountAmount;
    }

    /// <inheritdoc />
    public decimal CalculateDiscountedPrice(decimal originalPrice)
    {
        return Math.Max(0, originalPrice - _discountAmount);
    }
}
