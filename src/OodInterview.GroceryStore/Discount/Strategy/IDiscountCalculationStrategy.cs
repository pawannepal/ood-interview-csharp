namespace OodInterview.GroceryStore.Discount.Strategy;

/// <summary>
/// Interface for discount calculation strategies.
/// </summary>
public interface IDiscountCalculationStrategy
{
    /// <summary>
    /// Calculates the discounted price based on the original price.
    /// </summary>
    /// <param name="originalPrice">The original price.</param>
    /// <returns>The discounted price.</returns>
    decimal CalculateDiscountedPrice(decimal originalPrice);
}
