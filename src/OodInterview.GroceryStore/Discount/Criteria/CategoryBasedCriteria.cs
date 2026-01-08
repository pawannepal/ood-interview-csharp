namespace OodInterview.GroceryStore.Discount.Criteria;

/// <summary>
/// Criteria that applies discounts based on item category.
/// </summary>
public class CategoryBasedCriteria : IDiscountCriteria
{
    private readonly string _category;

    /// <summary>
    /// Creates a new category-based criteria with the specified category.
    /// </summary>
    /// <param name="category">The category to check against.</param>
    public CategoryBasedCriteria(string category)
    {
        _category = category;
    }

    /// <inheritdoc />
    public bool IsApplicable(Item item)
    {
        return item.Category.Equals(_category);
    }
}
