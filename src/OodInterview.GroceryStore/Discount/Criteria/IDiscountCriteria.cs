namespace OodInterview.GroceryStore.Discount.Criteria;

/// <summary>
/// Interface for discount criteria that determine if a discount applies to an item.
/// </summary>
public interface IDiscountCriteria
{
    /// <summary>
    /// Checks if the discount criteria applies to the given item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the discount applies to the item.</returns>
    bool IsApplicable(Item item);
}
