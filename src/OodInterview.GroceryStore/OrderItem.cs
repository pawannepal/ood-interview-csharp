using OodInterview.GroceryStore.Discount;

namespace OodInterview.GroceryStore;

/// <summary>
/// Represents an item in an order with its quantity.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Creates a new order item with the specified item and quantity.
    /// </summary>
    /// <param name="item">The item being ordered.</param>
    /// <param name="quantity">Quantity of the item.</param>
    public OrderItem(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    /// <summary>
    /// Gets the item being ordered.
    /// </summary>
    public Item Item { get; }

    /// <summary>
    /// Gets the quantity of the item.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Calculates the total price for this order item without any discount.
    /// </summary>
    /// <returns>The total price.</returns>
    public decimal CalculatePrice()
    {
        return Item.Price * Quantity;
    }

    /// <summary>
    /// Calculates the total price for this order item with the given discount.
    /// </summary>
    /// <param name="discount">The discount to apply.</param>
    /// <returns>The discounted price.</returns>
    public decimal CalculatePriceWithDiscount(DiscountCampaign discount)
    {
        return discount.CalculateDiscount(this);
    }
}
