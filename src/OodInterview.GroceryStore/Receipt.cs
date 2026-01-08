namespace OodInterview.GroceryStore;

/// <summary>
/// Represents a receipt for an order.
/// </summary>
public class Receipt
{
    private readonly Order _order;
    private readonly DateTime _issueDate;

    /// <summary>
    /// Creates a new receipt for the given order.
    /// </summary>
    /// <param name="order">The order to create a receipt for.</param>
    public Receipt(Order order)
    {
        ReceiptId = Guid.NewGuid().ToString();
        _order = order;
        _issueDate = DateTime.Now;
    }

    /// <summary>
    /// Gets the unique receipt ID.
    /// </summary>
    public string ReceiptId { get; }

    /// <summary>
    /// Prints the receipt as a formatted string.
    /// </summary>
    /// <returns>The formatted receipt string.</returns>
    public string PrintReceipt()
    {
        var receipt = new System.Text.StringBuilder();
        receipt.AppendLine($"Receipt ID: {ReceiptId}");
        receipt.AppendLine($"Date: {_issueDate}");
        receipt.AppendLine("Items:");

        foreach (var item in _order.Items)
        {
            receipt.Append($"- {item.Item.Name} x {item.Quantity} @ {item.Item.Price}");

            var discount = _order.AppliedDiscounts.GetValueOrDefault(item);
            if (discount == null)
            {
                receipt.AppendLine($" = {item.CalculatePrice()}");
            }
            else
            {
                receipt.Append($" ({discount.Name})");
                receipt.AppendLine($" = {item.CalculatePriceWithDiscount(discount)}");
            }
        }

        receipt.AppendLine($"Subtotal: {_order.CalculateSubtotal()}");
        receipt.AppendLine($"Total: {_order.CalculateTotal()}");

        return receipt.ToString();
    }
}
