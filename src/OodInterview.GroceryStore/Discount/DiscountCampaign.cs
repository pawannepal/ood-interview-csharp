using OodInterview.GroceryStore.Discount.Criteria;
using OodInterview.GroceryStore.Discount.Strategy;

namespace OodInterview.GroceryStore.Discount;

/// <summary>
/// Manages discount rules and calculations.
/// </summary>
public class DiscountCampaign
{
    private readonly IDiscountCriteria _criteria;
    private readonly IDiscountCalculationStrategy _calculationStrategy;

    /// <summary>
    /// Creates a new discount campaign with the specified details.
    /// </summary>
    /// <param name="discountId">Unique identifier for the discount campaign.</param>
    /// <param name="name">Name of the discount campaign.</param>
    /// <param name="criteria">Criteria that determines if the discount applies to an item.</param>
    /// <param name="calculationStrategy">Strategy for calculating the discounted price.</param>
    public DiscountCampaign(
        string discountId,
        string name,
        IDiscountCriteria criteria,
        IDiscountCalculationStrategy calculationStrategy)
    {
        DiscountId = discountId;
        Name = name;
        _criteria = criteria;
        _calculationStrategy = calculationStrategy;
    }

    /// <summary>
    /// Gets the unique identifier for the discount campaign.
    /// </summary>
    public string DiscountId { get; }

    /// <summary>
    /// Gets the name of the discount campaign.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Checks if this discount applies to the given item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the discount applies to the item.</returns>
    public bool IsApplicable(Item item)
    {
        return _criteria.IsApplicable(item);
    }

    /// <summary>
    /// Calculates the discounted price for the given order item.
    /// </summary>
    /// <param name="orderItem">The order item to calculate the discount for.</param>
    /// <returns>The discounted price.</returns>
    public decimal CalculateDiscount(OrderItem orderItem)
    {
        return _calculationStrategy.CalculateDiscountedPrice(orderItem.CalculatePrice());
    }
}
