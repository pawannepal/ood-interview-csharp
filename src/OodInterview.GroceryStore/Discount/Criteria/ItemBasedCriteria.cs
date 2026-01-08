namespace OodInterview.GroceryStore.Discount.Criteria;

/// <summary>
/// Criteria that applies discounts based on specific item barcode.
/// </summary>
public class ItemBasedCriteria : IDiscountCriteria
{
    private readonly string _itemId;

    /// <summary>
    /// Creates a new item-based criteria with the specified item barcode.
    /// </summary>
    /// <param name="itemId">The barcode of the item to check against.</param>
    public ItemBasedCriteria(string itemId)
    {
        _itemId = itemId;
    }

    /// <inheritdoc />
    public bool IsApplicable(Item item)
    {
        return item.Barcode.Equals(_itemId);
    }
}
