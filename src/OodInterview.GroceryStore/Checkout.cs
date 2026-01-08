using OodInterview.GroceryStore.Discount;

namespace OodInterview.GroceryStore;

/// <summary>
/// Handles the checkout process.
/// </summary>
public class Checkout
{
    private Order _currentOrder;
    private readonly List<DiscountCampaign> _activeDiscounts;

    /// <summary>
    /// Creates a new checkout with the given active discounts.
    /// </summary>
    /// <param name="activeDiscounts">The list of active discount campaigns.</param>
    public Checkout(List<DiscountCampaign> activeDiscounts)
    {
        _activeDiscounts = activeDiscounts;
        _currentOrder = new Order();
    }

    /// <summary>
    /// Starts a new order.
    /// </summary>
    public void StartNewOrder()
    {
        _currentOrder = new Order();
    }

    /// <summary>
    /// Processes the payment and returns the change.
    /// </summary>
    /// <param name="paymentAmount">The payment amount.</param>
    /// <returns>The change to return to the customer.</returns>
    public decimal ProcessPayment(decimal paymentAmount)
    {
        _currentOrder.SetPayment(paymentAmount);
        return _currentOrder.CalculateChange();
    }

    /// <summary>
    /// Adds an item to the current order and applies applicable discounts.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="quantity">The quantity to add.</param>
    public void AddItemToOrder(Item item, int quantity)
    {
        var orderItem = new OrderItem(item, quantity);
        _currentOrder.AddItem(orderItem);

        foreach (var newDiscount in _activeDiscounts)
        {
            if (newDiscount.IsApplicable(item))
            {
                // If there are multiple discounts that apply to item, apply the higher one
                var existingDiscount = _currentOrder.AppliedDiscounts.GetValueOrDefault(orderItem);
                if (existingDiscount != null)
                {
                    if (orderItem.CalculatePriceWithDiscount(newDiscount) >
                        orderItem.CalculatePriceWithDiscount(existingDiscount))
                    {
                        _currentOrder.ApplyDiscount(orderItem, newDiscount);
                    }
                }
                else
                {
                    _currentOrder.ApplyDiscount(orderItem, newDiscount);
                }
            }
        }
    }

    /// <summary>
    /// Generates a receipt for the current order.
    /// </summary>
    /// <returns>The receipt.</returns>
    public Receipt GetReceipt()
    {
        return new Receipt(_currentOrder);
    }

    /// <summary>
    /// Calculates the total amount for the current order.
    /// </summary>
    /// <returns>The order total.</returns>
    public decimal GetOrderTotal()
    {
        return _currentOrder.CalculateTotal();
    }
}
