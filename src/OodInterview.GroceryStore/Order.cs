using OodInterview.GroceryStore.Discount;

namespace OodInterview.GroceryStore;

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order
{
    private readonly List<OrderItem> _items = [];
    private readonly Dictionary<OrderItem, DiscountCampaign> _appliedDiscounts = [];
    private decimal _paymentAmount;

    /// <summary>
    /// Creates a new order with a random GUID.
    /// </summary>
    public Order()
    {
        OrderId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Gets the unique order ID.
    /// </summary>
    public string OrderId { get; }

    /// <summary>
    /// Gets the payment amount.
    /// </summary>
    public decimal PaymentAmount => _paymentAmount;

    /// <summary>
    /// Adds an item to the order.
    /// </summary>
    /// <param name="item">The order item to add.</param>
    public void AddItem(OrderItem item)
    {
        _items.Add(item);
    }

    /// <summary>
    /// Calculates the subtotal of all items without discounts.
    /// </summary>
    /// <returns>The subtotal.</returns>
    public decimal CalculateSubtotal()
    {
        return _items.Sum(item => item.CalculatePrice());
    }

    /// <summary>
    /// Calculates the total price including all applied discounts.
    /// </summary>
    /// <returns>The total price.</returns>
    public decimal CalculateTotal()
    {
        return _items.Sum(item =>
        {
            var discount = _appliedDiscounts.GetValueOrDefault(item);
            return discount != null ? item.CalculatePriceWithDiscount(discount) : item.CalculatePrice();
        });
    }

    /// <summary>
    /// Applies a discount to a specific item in the order.
    /// </summary>
    /// <param name="item">The order item.</param>
    /// <param name="discount">The discount to apply.</param>
    public void ApplyDiscount(OrderItem item, DiscountCampaign discount)
    {
        _appliedDiscounts[item] = discount;
    }

    /// <summary>
    /// Returns an unmodifiable list of items in the order.
    /// </summary>
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Sets the payment amount for the order.
    /// </summary>
    /// <param name="paymentAmount">The payment amount.</param>
    public void SetPayment(decimal paymentAmount)
    {
        _paymentAmount = paymentAmount;
    }

    /// <summary>
    /// Calculates the change to be returned to the customer.
    /// </summary>
    /// <returns>The change amount.</returns>
    public decimal CalculateChange()
    {
        return _paymentAmount - CalculateTotal();
    }

    /// <summary>
    /// Returns the map of applied discounts.
    /// </summary>
    public IReadOnlyDictionary<OrderItem, DiscountCampaign> AppliedDiscounts => _appliedDiscounts;
}
